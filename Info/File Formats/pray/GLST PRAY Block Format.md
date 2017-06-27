# GLST 
GLST blocks are found in .creature PRAY files and are Zlib-encoded CreaturesArchive data. User text on the conceived/engineered/spliced/cloned events define the Norn's description as in the Creatures 3/Docking Station UI. For the purposes of this article, 'null' is defined as a 4-byte integer with value 0. All strings in GLST blocks are preceded by a 4-byte integer stating their length. The length of a string can be 0. 

# Header and Footer 

GLST blocks are little-endian encoded, and all strings within them are not null-terminated. 

They begin as follows 

'  byte with value 0x29  
1  4-byte integer (1)  
moniker length  4-byte integer (always 32 (20))  
moniker  32-byte string  
moniker length  4-byte integer (always 32 (20))  
moniker  32-byte string (always seems to be identical to previous moniker)  
name length  4-byte integer  
name  n-byte string  
gender  4-byte integer (1 (m) or 2 (f))  
genus  4-byte integer  
species  4-byte integer  
number of events  4-byte integer  

There then is a series of events (as defined below) 


 The GLST block then ends with the following footer: 

Point mutations during conception (if creature was conceived. Seems to be totally different if the creature was not conceived)  4-byte integer  
Crossover points during conception (if creature was conceived. Seems to be totally different if the creature was not conceived)  4-byte integer  
Unknown  4-byte integer  
1 if the creature has been in the warp, 0 otherwise  4-byte integer  
String length  4-byte integer  
String (only seems to appear in eggs laid by Muco)  4-byte integer  

 Events 

GLST blocks contain events. In the table below, world time and creature age is specfied in ticks. The majority of the strings are optional. If they are not included, their length is 0. Associated monikers, associated photos, and user text are nearly always null. The event information below defines which events define which fields. User text is the description given by the user about the Norn. 

If creature age or Life stage is FF FF FF FF then the creature is yet to be born. 

Each event is encoded thusly: 

Event Number (numbers defined below)  4-byte integer  
World time  4-byte integer  
Creature age  4-byte integer  
UNIX timestamp  4-byte integer  
Life stage  4-byte integer  
Associated Moniker 1 length  4-byte integer  
Associated Moniker 1 (optional)  n-byte string  
Associated Moniker 2 length  4-byte integer  
Associated Moniker 2 (optional)  n-byte string  
User text length  4-byte integer  
User text (optional)  n-byte string  
Associated PHOT block name length  4-byte integer  
Associated PHOT block name (optional)  n-byte-string  
World name length  4-byte integer  
World name  n-byte string  
World Unique ID length (28)  4-byte integer  
World Unique ID  28-byte string  
Docking Station User ID length (8)  4-byte integer  
Docking Station User ID  8-byte string  
Unknown (usually if not always 1)  4-byte integer  
Unknown (usually/always null)  4-byte integer  

 Event Numbers 
0 - Conceived (associated monikers 1 and 2 are mother and father, respectively. User text is here.) 
1 - Creature spliced (associated monikers 1 and 2 are mother and father, respectively. Creature doesn't necessarily need a mother/father. This event occurs both in creatures spliced with the gene splicer and creatures from the egg layers. User text is here) 
2 - Engineered (associated moniker 2 is the genome the norn was based on. User text is here) 
3 - hatched (associated monikers 1 and 2 are mother and father, respectively) 
4 - Creature life stage 
5 - Creature exported 
6 - Creature imported 
7 - Died 
8 - Got pregnant (associated moniker 1 is the child, associated moniker 2 is the mother 
9 - Made another creature pregnant (associated moniker 1 is the child, associated moniker 2 is the mother) 
10 - Child born (associated moniker 1 is the child, associated moniker 2 is the other parent) 
11 - My egg was laid (associated moniker 1 is the mother) 
12 - Laid egg (associated moniker 1 is the baby) 
13 - Photo taken of me (associated PHOT block name is the photo that was taken) 
14 - I was cloned from another creature (associated moniker 1 is the original. User text is here.) 
15 - Another creature was cloned from me (associated moniker 1 is the clone) 
16 - Creature warped out 
17 - Creature warped in
