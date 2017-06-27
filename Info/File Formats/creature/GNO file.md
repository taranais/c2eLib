#GNO File Format

This information applies to c2e (C3/DS) only. All multi-byte values in the file are in little endian notation. 

GNO files contain notes to accompany GEN files.

The format is as follows: 

 BYTE[2] (Integer) Number of bytes to be read next = 2

 BYTE[2] (Integer) Number of SV notes

 SVNoteFileStruct[Number of SV notes]

 BYTE[2] (Integer) Number of bytes to be read next = 2

 BYTE[2] (Integer) Number of gene notes

 GeneNoteStruct[Number of gene notes]

##SVNotesStruct:

 BYTE[2] (Integer) Gene type

 BYTE[2] (Integer) Gene Sub-type

 BYTE[2] (Integer) Unique ID

 BYTE[2] (Integer) Rule Number

 STRING Annotation[16]

 STRING General Notes

##GeneNoteStruct:

 BYTE[2] (Integer) Gene type

 BYTE[2] (Integer) Gene Sub-type

 BYTE[2] (Integer) Unique ID

 STRING Caption

 STRING Rich Text Annotation

##STRING:

BYTE[2] (Integer) Number of bytes in string

BYTE[Number of bytes in string] Text string

#A BNF like description 

    <Caption-File>    ::= <SVRule-Section> <Caption-Section> <Encoding-Hints> 
    <SVRule-Section>  ::= <word-size> <count> count*<SV-Note> <zero-padding>

    <Caption-Section> ::= <word-size> <count> count*<Caption-Note> <zero-padding>
 
    <word-size> ::= 2-byte-integer
    <string>    ::= <count> count*<ASCII character>
    <count>     ::= word-sized-integer
 
 
    <SV-Note> ::= <gene-type>,
                   <gene-sub-type>,
                   <unique-id>,
                   <rule-number>
                   <unknown>,
                   <Annotation-List>,
                   <unknown>,
                   <general-note>

    <Caption-Note> := <gene-type>,
                       <gene-sub-type>,
                       <unique-id>,
                       <unknown>
                       <caption>,
                    <rich-text-annotation>
 
 
    <gene-type>       ::= word-sized-integer
    <gene-sub-type>   ::= word-sized-integer
    <unique-id>       ::= word-sized-integer
    <rule-number>     ::= word-sized-integer
 
    <unknown>         ::= word-sized-integer | <string>
 
    <Annotation-List> ::= 16*<string>
    <general-note>    ::= <string>
  
    <caption>         ::= <string>
  
    <rich-text-annotation> ::= <string>

    <zero-padding> ::= { '0x00' }


    <Encoding-Hints> ::= '0xDC050000'=1500,
                         '0x3F420F00'=999999,
                         '0x0D00'=13
                         '0x0200'=2
                         '0x0E00'=14

##Notes
+ The word-size values are two (0x0200) in all observed cases. 
+ The gene-type and gene-sub-type are the same as in genome files. 
+ The unique-id is value from 0 to 255, it is shown under the "G-ID" column in the Genetics Kit. It is the combination of gene type, sub-type, and this ID that uniquely identifies a caption and attaches the caption to a specific gene in the genome. 
+ The rule-number is either 0 or 1. Zero is the initialisation rule and one is the update rule. 
+ The sixteen annotations in an SV-Note are for each of the sixteen possible instructions in an SVRule. 
+ The Caption-Note's caption is shown under the "Description" column of the Genetics Kit and names the gene. 
+ The rich-text-annotation is shown with the "Notes" button on the gene editing dialog of the Genetics Kit. It is in Microsoft's RTF format. The original caption files provided with Creatures 3 only contain a single rich-text-annotation (for the chemical receptor gene captioned as "091 Belladonna poisoning - Receptor"). 
+ The unknown fields are zero (0x0000) in all observed cases. It is unclear if this is a reserved integer value or a reserved string value (of length zero). When writing a program to parse caption files care should be taken if these bytes are non-zero since if the value is meant to be interpreted as a string then the size of the enclosing SV-Note or Caption-Note will be different then expected. 

##Padding and "encoding" used by Genetics Kit

The zero-padding appears to be the correct number of zeros such that if they were interpreted as SV-Note or Caption-Note records (with zero-length strings) then there would be a total of 3001 records (of each type). It is believed that this number of records is related to the values in the Encoding-hints bytes at the end of the file. 

Reverse engineering of the Genetics Kit indicates that when opening a caption file it reads the last ten (10) bytes of the file first. If checks for the exact sequence for the integer value "999999" (as a magic number) and does some verification of the other values. The "13" and "14" values are thought to be some kind of record size indication, the genetics kit verifies that the two values differ by one. The "2" value is thought to be the word size or file format version number. 

If the values of the last ten bytes check out then the Genetics Kit reads the four bytes immediately preceding them. It is thought that this value is the array size and relates to there being enough zero padding for 3001 records, the genetics kit compares the read value to exactly "1500". At this point the Genetics Kit repositions to the beginning of the file and it either uses VBA routines to read arrays (if the Encoding-Hints match its expectations) or it falls back to reading the file record by record. 

Since all known caption files contain the padding, the "record-by-record" parsing method in the Genetics Kit is normally not used. It is therefore uncertain if it is as robust or error-free as the normal routines used to parse the file. When writing software to write a caption files this should be taken into account when considering if the padding method and undocumented bytes should be emulated or not.
