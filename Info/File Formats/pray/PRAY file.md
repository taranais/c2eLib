PRAY is a file format used by the Creatures Evolution Engine to store things such as agents (including starter families), exported creatures (with CreaturesArchive information stored inside), blueprints and egg information. 

Most of the functionality of Docking Station essentially works by transferring PRAY files containing informational blocks or creatures over TCP/IP (NetBabel). 

It is similar to the COB format used in earlier games in the Creatures series in that it uses blocks, however it is designed to be more flexible and extensible. 

You can decompile PRAY files using REVELATION or Zeus, and compile them using various methods, such as Pray Builder, EasyPRAY or the engine itself. 
 
 File Format 

The file starts with a four-byte id - "PRAY" in ASCII - followed by a set of blocks, one after another, which tools should continue parsing until they hit the end of the file, skipping blocks they don't know how to parse/use. 

Blocks consist of a block header, followed by block data which is specific to certain types of blocks. All data is in little endian format. 

While PRAY blocks often contain binary data which tools will have to parse/create using custom code, there is an often-used 'tag' data format which many PRAY blocks use (such as AGNT, AUTH, EGGS and EXPC). 

 Block Header 


type 

description 

4 bytes  Defines the type of the block, should consist of ASCII characters (eg, "FILE").  
128 bytes  Null-terminated string containing the name of the block, such as a filename. The remaining space is padded with zeros.  
32-bit integer  The length, in bytes, of the block-specific data following this block.  
32-bit integer  The length, in bytes, of the block-specific data after it has been decompressed. Should be the same as the previous value if uncompressed.  
32-bit integer  Bit 1 should be set if the block is compressed using Zlib. All other bits are unused and should be set to zero.  




 FILE blocks 

A block of type 'FILE' contains a file, typically used by agent blocks; the name of the block is the filename, and the block data is the file contents. 

 'tag' PRAY block format 

This block data consists of integer and string values with associated names. A typical string value might have the name "Creature Name" and the value "Alice"; a typical integer value might have the name "Creature Life Stage" and the value 2. 

The data is stored as follows: 

32-bit integer  The number of integer values.  
For each integer value:  
 32-bit integer  The length of the 'name' associated with this integer value.  
 string  The 'name' associated with this integer value, with the length given in the previous integer.  
 32-bit integer  The actual integer value.  




32-bit integer  The number of string values.  
For each string value:  
 32-bit integer  The length of the 'name' associated with this string value.  
 string  The 'name' associated with this string value, with the length given in the previous integer.  
 32-bit integer  The length of the string value.  
 string  The actual string value, with the length given in the previous integer.  

 EGG blocks 

Used in egg files. 

 DFAM blocks 

Used in Docking Station 'Starter Family' files. 

 SFAM blocks 

Used in Creatures 3 'Starter Family' files. 

 AGNT/DSAG/LIVE blocks 

These blocks contain agents - both the code and information about them, such as dependencies. 
 AGNT is Creatures 3 
 DSAG is Docking Station 
 LIVE is Sea-Monkeys 

 EXPC/DSEX blocks 

These blocks contain information about creatures exported from Creatures 3 (for EXPC) or Docking Station (for DSEX). 

 GLST blocks 

CreaturesArchive data, used in exported creature and Starter Family files. GLST blocks contain a creature's life history including each transition to a new life stage. 

The GLST format is documented here. 

 CREA blocks 

CreaturesArchive data representing a serialized creature. Used in exported creature and Starter Family files. 

 GENE blocks 

The data normally contained within a genome file. Used in exported creature and Starter Family files. 

 PHOT blocks 

An .s16 file with a single frame, containing a photograph of a creature created with SNAP. Used in exported creature and Starter Family files.
