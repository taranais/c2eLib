
# Shared Memory Interface

The Creatures 3 game engine, used by Creatures Adventures and Creatures 3, uses shared memory, events and mutexes to interface with third party programs. For a good introduction to using shared memory on the Windows platform see the Wicked Code column in the November 1998 issue of Microsoft Systems Journal. It's what I used to get familiar with it.

All of the C3 engine games are identified using a game name. In Creatures 3 and Creatures Adventures the game name is passed as a command line argument to the engine itself. The game name is used to identify what registry entries are used for the game and how to access the shared memory interface. The following are the game names I know of at this stage:


|Game | Game Name |
|-----|-----------|
|Creatures Adventures | Creatures Village |
|Creatures 3 | Creatures 3 |

The game name is used to create global objects that other programs can access to allow communication with the engine. The global objects created by the engine are:


|Object Type | Creatures 3 Object Name | Creatures Adventures Object Name |
|------------|-------------------------|----------------------------------|
|Mutex | Creatures 3_mutex | Creatures Village_mutex |
|Event | Creatures 3_request | Creatures Village_request |
|Event | Creatures 3_result  | Creatures Village_result |
|Shared Memory | Creatures 3_mem | Creatures Village_mem |

The steps to communicate with the game engine involve:

1. Retrieve handles to the global objects.
2. Acquire the mutex so other programs cannot write to the buffer.
3. Fill the shared memory buffer with the request.
4. Signal the request event.
5. Wait on the result event.
6. Retrieve the results from the shared memory buffer.
7. Release the mutex.

The mutex is used to lock the shared memory buffer when writing or reading from it to ensure that nothing changes due to other applications writing to it at the time. It is very important to acquire the mutex before writing to the buffer and to release it as soon as you've finished reading the results from the buffer. No other program can access the shared memory interface while you have the mutex locked.

The first step required is to retrieve handles to the global objects. The following pseudo code shows the Windows API calls required to do this. Note that the game name used in the example is for Creatures 3, and I don't do any error checking in the example.

## Retrieving Object Handles

### C++

    HANDLE memory_handle = OpenFileMapping(MAP_FILE_ALL_ACCESS, FALSE, "Creatures 3_mem");
    unsigned char* memory_ptr = MapViewOfFile(memory_handle, FILE_MAP_ALL_ACCESS, 0, 0, 0);
    HANDLE mutex = OpenMutex(MUTEX_ALL_ACCESS, FALSE, "Creatures 3_mutex");
    HANDLE result_event = OpenEvent(EVENT_ALL_ACCESS, FALSE, "Creatures 3_result");
    HANDLE request_event = OpenEvent(EVENT_ALL_ACCESS, FALSE, "Creatures 3_request");

When the above code is executed the memory_ptr variable will point to the shared memory buffer. The elements of this can be read and written to just like a normal area of memory. This buffer holds the request to be sent to the game engine and the results that are returned. It is a 'C struct' with the following memory layout:


| Offset | Size     | C Type       | Memory Buffer Layout
|--------|----------|--------------|---------------------------------
| 0      | 4        | CHAR         |4 characters which should be 'c2e@'. If it is not this then either the buffer is corrupt or you're looking in the wrong place.
| 4      | 4        | DWORD        | Process id of the game engine. By retrieving this you can use Windows API functions to ensure that the game engine is still running, or be notified if it is closed, when waiting for results.
| 8      | 4        | int          | Result code from the last game engine request submitted. A '0' is success, a '1' is failed. If there is a failure the reason for the failure is in the data section below.
| 12     | 4        | unsigned int | The size in bytes of the data returned in the data buffer.
| 16     | 4        | unsigned int | The size of the shared memory buffer. No requests or replies can be larger than this. It is currently set to about 1MB in the game engine.
| 20     | 4        | int          | Padding to align data on an 8 byte boundary.
| 24     | variable |              | Request dependant This is where you store your request and retrieve your replies. Depending on the type of request it can be a null terminated array of characters or binary data.

When you want to get the game engine to do something you must populate the data portion of the shared memory buffer starting from offset 24 as described in the previous table. The format of this request buffer depends on what you want to do. The following table describes the options that I know of.


| Task | Request format | Result format
|------|----------------|--------------
|Executing CAOS scripts and retrieving results. | The data portion of the buffer must start with the string "execute\n" followed by the null terminated text of the CAOS you want to execute. For example, "execute\noutv 99". The '\n' is a code for an ASCII carriage return - not the actual characters '\' followed by '\n'. The byte value of 13 can be used. | The result of the script will appear in the data portion of the buffer as a null terminated string for most CAOS commands. For CAOS commands detailed as having 'binary' results, the binary data will appear in the data portion of the buffer with the length of the binary data at offset 12 in the buffer. The format of the binary data varies among the CAOS commands.
|Adding scripts to the scriptorium  | The data portion of the buffer must start with the string "scrp a b c d\n" followed by the null terminated contents for that script. The script must not end with an 'endm', the null termination is used. The 'a b c d' should be the family, genus, species and event respectively. This script will be installed or replace any existing script with that classification. As in the previous example, '\n' is an ASCII carriage return. | If an error occurs, the error message will appear in the data portion of the buffer.

Don't forget to acquire the mutex before using the buffer!

Once the buffer is filled with the information you want for the request you need to signal the request event and wait for the result event to happen. You should also wait on the process handle of the game engine (available from the shared memory buffer) to check for the case of the game engine being closed while waiting for an event.

Here is a example of executing CAOS in a C like language, continuing from the previous example of retrieving object handles.

## Executing CAOS

    WaitForSingleObject(mutex, INFINITE);
    unsigned char* buffer_start = (unsigned char*)memory_ptr + 24;
    strcpy(buffer_start, "execute\noutv 99");
    ResetEvent(result_event);
    PulseEvent(request_event);
    DWORD server_process_id = *(DWORD*)(memory_ptr + 4);
    HANDLE process_handle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, server_process_id);
    HANDLE wait_handles[2];
    wait_handles[0] = process_handle;
    wait_handles[1] = result_event;
    WaitForMultipleObjects(2, wait_handles, FALSE, INFINITE);
    // When the WaitForMultipleObjects returns, the buffer will hold the results.
    // Process the results quickly and release the mutex. It may be best to
    // copy the result data so you can release the mutex as quickly as possible.
    CloseHandle(process_handle);
    ReleaseMutex(mutex);

The example above outputs '99' in the data portion of the result buffer. Note that you MUST release the mutex when finished reading from the buffer. If you do not, no other program can access the game engine.

You must cleanup by closing all the handles to the objects created above when finished with the connection to the game engine.


## Cleaning Up

    CloseHandle(result_event);
    CloseHandle(request_event);
    CloseHandle(mutex);
    UnmapViewOfFile(memory_ptr);
    CloseHandle(memory_handle);

And that's all there is to it. Remember that the code above is not robust. You should check for error results from the API calls, and ensure that you release the mutex if you have acquired it.

Acknowledgements

double.co.nz/creatures/developer/sharedmemory.htm