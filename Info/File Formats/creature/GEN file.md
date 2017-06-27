# GEN file format
## General terms and definitions

### BYTE

BYTE defines a single byte of data. An 8 bit unsigned integer, a BYTE can store whole numbers ranging from zero to 255. 

### Arrays

Arrays of bytes are defined as BYTE[SIZE] and contain SIZE bytes of data. 

## A Word about this document

This document is not meant to teach you genetics. Nor is it meant to tell you how to interpret the values within the genome. It is to explain the file format,    and to give you a brief insight into how the genetics data is constructed,  so  that you can write utilities which operate on the genetics files.

## The overall file format

The genetics file consists of a magic cookie of four characters at the beginning   of the file, and then a series of genes, delimited by gene markers, terminated    with an end token.  

* The cookie is:        
    BYTE[4] Cookie = 'd' 'n' 'a' '3'
* The gene delimiter is:
    BYTE[4] Delimiter = 'g' 'e' 'n' 'e'
* The end marker is:        
    BYTE[4] EndMarker = 'g' 'e' 'n' 'd' 

## The genetic hierarchy 

Creatures in Creatures 3 have genetically specified organs, which contain certain   genes. These organs and the genes which the contain are in a certain configuration   within the genetics files.
 
Biochemical Reactions, Receptors and Emitters all belong to the organ which  was last defined. If no organ has yet been defined then they are considered "orphaned" and belong to the implied "body" organ. Brain lobes belong to the  "TheBrain Organ" and again, if no brain organ definition has been encountered   then they are "orphaned" and belong to the next brain organ to be defined.  Note that should you define multiple Brain Organs, only the very last one to  be defined  will be expressed. A creature can have only one brain. There must be only one "Creature/Genus" gene, which must be gene zero in the  file. If this is not so, then the genetics file is invalid. All other types of genes belong to the body and their order does not matter. 

## The structure of a gene

Genes each have a header, and a body. The header is of a fixed length, the   body of the gene varies in length between gene types, but is consistent within a gene type. 

### The gene Header

1. BYTE GeneType
2. BYTE GeneSubType
3. BYTE GeneID
4. BYTE Generation
5. BYTE SwitchOnTime    
6. BYTE Sex-Dependance/Mutability-Flags
7. BYTE MutabilityWeighting
8. BYTE Expression Variant

#### The Generation, SwitchOn and Flags entries

The generation byte indicates how many times this gene has been duplicated. The switch on time ranges from 0 (embryo) to 6 (senile). The flags entry is a bit field which is arranged as follows. 

|BIT|Meaning      |
|:-:|:-----------:|
|7  |Reserverd    |
|6  |Reserverd    |
|5  |Not Expressed|
|4  |Female only  |
|3  |Male Only    |
|2  |Deletable    |
|1  |Duplicable   |
|0  |Mutable      |
      
#### The Mutability Weighting entry
currently undocumented.... oooooooooh...
 
##Brain Genes (Type 00)

There are three types of brain genes, the first is the brain lobe (Type 00 00), the second, the brain organ (type 00 01), and the third the brain tract (type 00 02). 

##Brain Lobes (Type 00 00 - Size 121) 
In C3 Brain lobe genes perform all their calculations by means of SV rules.

    BYTE [4] Lobe ID
    BYTE [2] Update Time
    BYTE [2] x
    BYTE [2] y
    BYTE Width
    BYTE Height
    BYTE Red
    BYTE Green
    BYTE Blue
    BYTE WTA
    BYTE Tissue
    BYTE [8] spare
    BYTE [48] Initialise Neuron Rule
    BYTE [48] Update Neuron Rule

###Brain Lobe Information 
+Lobe id is a character based token
+Width * Height define the number of neurons in the lobe.
+The Flags entry in the brain lobe indicates whether the lobe is normal (0) or a Winner-Takes-All (1) Lobe.
+Tissue numbers are used to link lobes to emitters and receptors
+An SV rule consists of 16 entries. Each entry is of the following format:

    BYTE Opcode token
    BYTE Operand token
    BYTE Value/Chemical Token/Variable Token 

##Brain Organs (Type 00 01 - Size 5)

The Brain organ (of which there should be only one in any genetics file) specifies the properties of the brain organ expressed in the creature. It has the same internal format as the other organs, but a different type identifier. 

    BYTE ClockRate
    BYTE RepairRate
    BYTE LifeForce
    BYTE BioTickStart
    BYTE ATPDamageCoEff

##Brain Tract (Type 00 02 - Size 128)

Tract define connections between lobes and detail the number of dendrites and the style in which they are attached and migrate. 

    BYTE [2] Update Time
    BYTE [4] Source Lobe ID
    BYTE [2] Source Lobe Lower Bound
    BYTE [2] Source Lobe Upper Bound
    BYTE [2] No Source Connections
    BYTE [4] Destination Lobe ID
    BYTE [2] Destination Lobe Lower Bound
    BYTE [2] Destination Lobe Upper Bound
    BYTE [2] No Destination Connections
    BYTE Use Random and Migrating Dendrites
    BYTE No Of Connections Is Random
    BYTE [8] spare
    BYTE [48] Initialize Dendrite Rule
    BYTE [48] Update Dendrite Rule

###Brain Tract Information

An SV rule consists of 16 entries. Each entry is of the following format: 

    BYTE Opcode token
    BYTE Operand token
    BYTE Value/Chemical Token/Variable Token 


##Biochemistry Genes (Type 01) 

The biochemistry genes govern the functions of the chemicals within the body. They govern reactions, interfaces between the brain and the chemicals, and much, much more.

 ##Biochemical Receptors (Type 01 00 - Size 8) 

The biochemical receptors latch onto a locus within the brain or the current organ, and provide feedback to the brain about chemicals within the body. 

    BYTE Organ
    BYTE Tissue
    BYTE Locus
    BYTE Chemical
    BYTE Threshold
    BYTE Nominal
    BYTE Gain
    BYTE Flags

The flags indicate if the receptor output is inverted (1) or if it is digital (2). 

A digital receptor has its state calculated as: 

    Output = Nominal ± Gain If Signal>Threshold

##Biochemical Emitters (Type 01 01 - Size 8) 

Emitters tie to loci2 as do receptors. They read the value of the locus, and emit chemicals based on their input states. 

    BYTE Organ
    BYTE Tissue
    BYTE Locus
    BYTE Chemical
    BYTE Threshold
    BYTE Rate
    BYTE Gain
    BYTE Flags

 The Emitter flags can be: 1: Clear input signal after reading or 2: Invert Input Signal 

## Biochemical Reactions (Type 01 02 - Size 9)  

These genes govern the reactions, which occur within the Biochemical Sea of the creature. The only type of reaction which is explicitly prevented is a "Nothing -> Something" reaction. 

    BYTE Reactant0
    BYTE Quantity0
    BYTE Reactant1
    BYTE Quantity1
    BYTE Product2
    BYTE Quantity2
    BYTE Product3
    BYTE Quantity3
    BYTE ReactionRate

 The Reaction rate is such that 0=instant, 255=52 years 

## Biochemical Half Lives (Type 01 03 - Size 256) 

These are the decay rates of all of the chemicals within the creature's Biochemical Sea. They are arranged in the same way as the reaction rate. 

    BYTE [256] Chemical-Half-Life 

## Biochemical Initial Concentrations (Type 01 04 - Size 2)

These genes allow initial amounts of certain chemicals to be present in the creature at birth, giving a certain level of energy etc to the new born. 

    BYTE Chemical
    BYTE Amount

## Biochemical Neuro Emitters (Type 01 05 - Size 15)

Neuro Emitters tie to loci2 as do receptors. They emit set amounts of 3 chemicals when the three specified neurons fire. 

    BYTE Lobe #1
    BYTE Neuron # in lobe #1
    BYTE Lobe #2
    BYTE Neuron # in lobe #2
    BYTE Lobe #3
    BYTE Neuron # in lobe #3
    BYTE Sample Rate
    BYTE Chemcal Emitted #1
    BYTE Amount Emitted for Chemical #1
    BYTE Chemcal Emitted #2
    BYTE Amount Emitted for Chemical #2
    BYTE Chemcal Emitted #3
    BYTE Amount Emitted for Chemical #3
    BYTE Chemcal Emitted #4
    BYTE Amount Emitted for Chemical #4

## Creature Genes (Type 02)  

Creature genes govern sections of the creature such as walking gaits, poses and appearance.

###Creature Stimulus (Type 02 00 - Size 13)  

This is the genetically specified result of a specific stimulus being applied to the creature. For example, releasing pain into the drive 1 when hit. 

    BYTE StimulusNumber
    BYTE Significance
    BYTE Sensory-Neurone
    BYTE Features
    BYTE Drive0
    BYTE Amount0
    BYTE Drive1
    BYTE Amount1
    BYTE Drive2
    BYTE Amount2
    BYTE Drive3
    BYTE Amount3

### Creature Genus (Type 02 01 - Size 65)  

This gene specifies the type of creature, and the monikers of its parents. 

    BYTE Genus
    BYTE [32] MumMoniker 
    BYTE [32] DadMoniker

### Creature Appearance (Type 02 02 - Size 3)  

This specifies the appearance of the parts of the creature's body. 

    BYTE BodyPart
    BYTE Variant
    BYTE GenusOfDonor

 The GenusOfDonor entry allows the creation of creatures with grendel legs, ettin head and tail, and a norn body and arms, whilst being an ettin (If you see what I mean

### Creature Pose (Type 02 03 - Size 17) 

This is the arrangement of body parts for a specific pose. Note: C3 uses 1 extra byte in the pose string than C2. 

    BYTE PoseNumber
    BYTE [16] PoseString (last byte is spare)

 The Pose String can be generated from the Pose Editor.

### Creature Gait (Type 02 04 - Size 9) 

This is a collection of poses which show movement. 

    BYTE GaitNumber //A Drive
    BYTE [8] PoseSequence

### Creature Instinct (Type 02 05 - Size 9) 

These are processed during embryology and whilst the creature is asleep. They provide a way of providing the creature wish some basic information like "Eat Food When Hungry = Good" and "Do Nothing When Bored = Bad"

    BYTE Lobe0
    BYTE Neuron0
    BYTE Lobe1
    BYTE Neuron1
    BYTE Lobe2
    BYTE Neuron2
    BYTE Action
    BYTE ReinforcementDrive
    BYTE ReinforcementLevel

### Creature Pigments (Type 02 06 - Size 2)

These simple genes are used to give the creature a variation of colouring from the base colour of the variant sprites.

    BYTE PigmentColour
    BYTE AmountOfColour

### Creature Pigment Bleeds (Type 02 07 - Size 2)

These alter the colours specified by the pigments. They are used to again, provide some way of varying appearance.

    BYTE Rotation
    BYTE Swap

## Organs (Type 03)

Organs are the genes which specify the organ properties. Organs include the heart, skin, bones etc.

### Organs Organ (Type 03 00 - Size 5)

These have the same structure as the Brain Organ. Organs contain all the receptors, emitters and reactions listed after them in the genetics file (or until another organ gene is encountered). Reactions etc, contained within the organ, will only function whilst the organ is alive.

    BYTE ClockRate
    BYTE RepairRate
    BYTE BioTickStart
    BYTE ATPDamageCoEff

##Appendix One - SVRules 

SVRule (Or State-Variable Rules) are a sequence of 16 entries each of which is 3 bytes. Each entry is of the following format:

    BYTE Opcode token
    BYTE Operand token
    BYTE Value/Chemical Token/Variable Token

These rules allow the calculation of a state (strength gain, etc), from predetermined values. The following are the valid SVRule tokens.

Opcodes
•0 "stop"
•1 "blank", _
•2 "store in"
•3 "load from", _
•4 "if ="
•5 "if <>"
•6 "if >"
•7 "if <"
•8 "if >="
•9 "if <="
•10 "if zero"
•11 "if non-zero"
•12 "if positive"
•13 "if negative"
•14 "if non-negative"
•15 "if non-positive" _
•16 "add"
•17 "subtract"
•18 "subtract from"
•19 "multiply by"
•20 "divide by"
•21 "divide into"
•22 "minimum with"
•23 "maximum with" _
•24 "set tend rate"
•25 "tend to"
•26 "load negation of"
•27 "load abs of"
•28 "get distance to"
•29 "flip around"
•30 "no operation"
•31 "register as spare"
•32 "bound in [0,1]"
•33 "bound in [-1,1]"
•34 "add and store in"
•35 "tend to, store in"

Operands
•0 "accumulator"
•1 "input neuron"
•2 "dendrite"
•3 "neuron"
•4 "spare neuron"
•5 "random",
•6 "source chemical"
•7 "chemical"
•8 "destination chemical"
•9 "zero"
•10 "one"
•11 "value"
•12 "negative value"

Chemical Tokens

Taken in order from the chemical list

Variable Tokens

Taken in order from the variable list

Appendix Two - Loci

The following information was taken from the Creatures2 SDK documentation by Mark Ashton.

Receptor Loci ( type = 0)


 'Organ'
 
Tissue
 
Locus ID
 
Description
 

The use of the term 'Organ' here is confusing - here it is used to determine between brain and body, and has nothing to do with organs defined in the genome.

The loci for genetically defined organs are not accessible through the LCUS command
 

0 Brain
 
Use lobe number
 
0 LOCUS_THRESHOLD
 
Cell threshold
 
  
1 LOCUS_LEAKAGE
 
Cell leakage rate
 
  
2 LOCUS_RESTSTATE
 
Cell rest state
 
  
3 LOCUS_RELAXSUS0
 
Dentype 0 susceptibility decay rate
 
  
4 LOCUS_RELAXSTW0
 
Dentype 0 Short-term synapse decay rate
 
  
5 LOCUS_LTWRATE0
 
Dentype 0 synaptic consolidation rate
 
  
6 LOCUS_GAINSTRENGTH0
 
Dentype 0 linear strength gain rate
 
  
7 LOCUS_LOSESTRENGTH0
 
Dentype 0 linear strength loss rate
 
  
8 LOCUS_RELAXSUS1
 
Dentype 1 susceptibility decay rate
 
  
9 LOCUS_RELAXSTW1
 
Dentype 1 Short-term synapse decay rate
 
  
10 LOCUS_LTWRATE1
 
Dentype 1 synaptic consolidation rate
 
  
11 LOCUS_GAINSTRENGTH1
 
Dentype 1 linear strength gain rate
 
  
12 LOCUS_LOSESTRENGTH1
 
Dentype 1 linear strength loss rate
 
  
13 LOCUS_CHEM0
 
General-purpose loci for use by svrules.
 
  
14 LOCUS_CHEM1
  
  
15 LOCUS_CHEM2
  
  
16 LOCUS_CHEM3
  
  
17 LOCUS_CHEM4
  
  
18 LOCUS_CHEM5
  




 'Organ'
 
Tissue
 
Locus ID
 
Description
 

0 Brain
 
Use Lobe Number
 
19 LOCUS_STATE0
 
State of first cell in lobe - other cells follow on.

I.e. LOCUS_STATE0 + cellnum
 

1 Creature
 
0 Somatic
 
0 LOC_AGE0
 
If on and currently AGE_BABY then become AGE_CHILD.
 
  
1 LOC_AGE1
 
If on and currently AGE_CHILD, become AGE_ADOLESCENT
 
  
2 LOC_AGE2
  
  
3 LOC_AGE3
  
  
4 LOC_AGE4
  
  
5 LOC_AGE5
 
AGE_ADULT -> AGE_SENILE
 
  
6 LOC_AGE6
 
If on, die immediately of old age.
 
 
1 Circulatory
 
0 LOC_FLOATING0
 
General purpose emitter and receptor loci for doing more complex chemical regulation.
 
  
1 LOC_FLOATING1
  
  
2 LOC_FLOATING2
  
  
3 LOC_FLOATING3
  
  
4 LOC_FLOATING4
  
  
5 LOC_FLOATING5
  
  
6 LOC_FLOATING6
  
  
7 LOC_FLOATING7
  
 
2 Reproductive
 
0 LOC_OVULATE
 
If low, remove any egg/sperm from gamete; if high add one.
 
  
1 LOC_RECEPTIVE
 
If >0, female is receptive to incoming sperm and will conceive.
 
  
2 LOC_CHANCEOFMUTATION
  
  
3 LOC_DEGREEOFMUTATION
  
 
3 Immune
 
0 LOC_DIE
 
If on, creature dies (ill health, poison, stavation)
 






 'Organ'
 
Tissue
 
Locus ID
 
Description
 

2 Creature
 
4 Sensorimotor
 
0 LOC_INVOLUNTARY0
 
Trigger involuntary actions (fits, flinches etc)
 
  
.
  
  
7 LOC_INVOLUNTARY7
  
  
8 LOC_GAIT0
 
Trigger various walking gaits (0=default, usually no need for a receptor here).
 
    
  
24 LOC_GAIT16
  
 
5 Drives
 
0 LOC_DRIVE0
 
Drive levels (both receptors and emitters)
 
    
  
16 LOC_DRIVE16
  



Emitter loci (type = 1)


 'Organ'
 
Tissue
 
Locus ID
 
Description
 

0 Brain
 
Use lobe number
 
0 LOCUS_ACTIVITY
 
Current amount of cell activity in lobe (# cells firing)
 
  
1 LOCUS_NUMLOOSE0
 
Number of loose dens/cell in lobe (type 0 dens)
 
  
2 LOCUS_NUMLOOSE1
 
Ditto for type 1 dens
 
  
3 LOCUS_OUTPUT0
 
Output for first cell in lobe.

Get other cells in lobe using LOCUS_OUTPUT0 + cellnumber as an ID
 
  
4.

Other cell outputs follow on from here - one locus per cell.
  

1 Creature
 
0 Somatic
 
0 LOC_MUSCLES
 
How much energy has been expanded on movement this tick
 
    
 
1 Circulatory
 
0 LOC_FLOATING0
 
General purpose emitter and receptor loci for doing more complex chemical regulation.
 
  
1 LOC_FLOATING1
  
  
2 LOC_FLOATING2
  
  
3 LOC_FLOATING3
  
  
4 LOC_FLOATING4
  
  
5 LOC_FLOATING5
  
  
6 LOC_FLOATING6
  
  
7 LOC_FLOATING7
  
 
2 Reproductive
 
0 LOC_FERTILE
 
255 if a male has sperm or a female has an egg available
 
  
1 LOC_PREGNANT
 
255 if female has both egg and sperm so is pregnant
 
 
3 Immune
 
0 LOC_DEAD
 
>0 if creature is dead (allows post-mortem chemistry)
 




 'Organ'
 
Tissue
 
Locus ID
 
Description
 
 
4 Sensorimotor
 
0 LOC_CONST
 
Constant 255 (for regular emitters)
 
  
1 LOC_ASLEEP
 
255 if asleep, else 0
 
  
2 LOC_COLDNESS
 
How far air temp is below blood temp
 
  
3 LOC_HOTNESS
 
How far air temp is above blood temp
 
  
4 LOC_LIGHTLEVEL
 
How bright the sky is (eg control sleepiness)
 
  
5 LOC_CROWDEDNESS
 
How many and how close others of your kind are.
 
  
6 LOC_RADIATION
  
  
7 LOC_TIMEOFDAY
  
  
8 LOC_SEASON
  
  
9 LOC_AIRQUALITY
  
  
10 LOC_UPSLOPE
 
How steep is the slope I'm facing?
 
  
11 LOC_DOWNSLOPE
  
  
12 LOC_HEADWIND
 
Speed of wind coming toward me.
 
  
13 LOC_TAILWIND
 
Speed of wind coming from behind me.
 
 
5 Drives
 
0 LOC_DRIVE0
 
Drive levels (both receptors and emitters)
 
    
  
16 LOC_DRIVE16
  
