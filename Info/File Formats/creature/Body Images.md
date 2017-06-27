# Images and Attachments

This article details the formula used when creating image files and attachments for brained-creatures.

The sprite and attachment files for a creature are named using a formula which describes the body part, gender, stage of life and variant. The table below shows the values:

LXYZ . extension
    L: Body Part
    X: Gender
    Y: Stage of Life
    Z: Variant
    .extension: s16, c16 or ATT



|Body Part| Gender| Stage of Life | Norn Variants|
|---------|-------|---------------|--------------|
|A: Head  |0: Male Norn| 0: Baby  | A: Bruin     |
|B: Body  |1: Male Grendel| 1: Child| B: Bengal  |
|C: Left thigh |2: Male Ettin| 2: Adolescent| C: Civet
|D: Left shin  |3: Male Geat |3: Youth||
|E: Left foot  |4: Female Norn| 4: Adult||
|F: Right thigh| 5: Female Grendel| 5: Old||
|G: Right shin |6: Female Ettin |6: Senile||
|H: Right foot |7: Female Geat|||
|I: Left humerus||||
|J: Left radius||||
|K: Right humerus||||
|L: Right radius||||
|M: Tail root||||
|N: Tail tip||||


## Sprite File Arrangement

Each sprite file for each body part needs to be arranged in a consistent order to allow the correct facial expressions, for example, to be displayed.

The ordering that is used can be summed up by describing it as having each body part arranged in the following order:

Face Left - 4 poses
Face Right - 4 poses
Face Front - 4 poses
Face Back - 4 poses.

But this doesn't describe the specifics of, for example, how the four poses for each direction are arranged - or mention how the head graphics use the previous ordering per facial expression. The easiest way to explain is to illustrate it, so each body part in the table above is linked to an example image showing the ordering of the parts of the sprite file. Click on the Body Part name for the image in a pop-up window.

### Pose Genes

Pose Genes are made up of a 15 character string, the ordering of each element is as follows: Direction, Head, Body, Left Thigh, Left Shin, Left Foot, Right Thigh, Right Shin, Right Foot, Left Humerus, Left Radius, Right Humerus, Right Radius, Tail Root, Tail Tip.

The meaning of each pose gene element is explained below:

Direction - 0: Face away from screen
Direction - 1: Face out of screen
Direction - 2: Face right
Direction - 3: Face left
Direction: - ?: Face towards _IT_
Direction - !: Face away from _IT_
Head - ?: Look towards _IT_

For all Parts - 0, 1, 2, 3: Furthest down/back pose to furthest up/forward pose. Each body part has 4 degrees of rotation on it to the left and right, forward and backwards.

For all Parts - X: No change in part arrangement


### The Seven Stages of Norn

Each creature has 7 life stages that they pass through, these are:

Stage of Life Number
Baby 0
Child 1
Adolescent 2
Youth 3
Adult 4
Old 5
Senile 6

These numbers are used for sprite/attachment naming (as mentioned above), but also come into play during genome creation - switch on times, for example. Not all life stages need a graphical representation - and as shipped C3/DS does not use them all. Instead the creature will use graphics for the previous life stage, if it can not find ones specifically for the stage it is in now.

## Understanding and Editing ATTs

A Creature's genome tells it which breed slot will be used for each body part, and sprites give the breed its appearance. But the information that tells the game where the body parts connect to one another is stored in the .att files.

ATTs can appear daunting at first glance, but the information they contain is actually very simple, and once you know how they work you may find that they are fairly easy to edit. This tutorial is designed to help you understand how ATTs work, and what to do if you want to edit them.

Part one covers the basics of how an ATT file is used by the game and how the information is stored. Part two explains how to determine new body part coordinates efficiently using a graphics program such as Photoshop.


Part 1

What an ATT file does

An ATT, also called a body data file, shares the exact same name as the sprite file it corresponds to (see an explanation of the naming convention here). So, for instance, a head sprite file named a04a.c16 has a matching a04a.att file. The game engine itself tells a Creature which body parts are linked to each other, the body data files indicate where the link is located, and the Creature's genome indicates which parts appear in which positions for a given pose.

Knowing this, we can be certain that the numbers within an .att file consist solely of joint coordinates, because all of the other data needed to put a Creature together and make it move is already handled somewhere else.


What the numbers mean

Take a look at an .att file for a Norn's head. File "a04a.att" or "a04d.att", which correspond to the Bruin and Chichi heads, are good examples. You can open them right up in a basic word processor, such as Notepad (in fact this kind of program is preferred for editing ATT files). As you can see, there are 16 lines in the file, each with 10 numbers, which are separated by spaces. The last six numbers on a line are all zeroes. Only the first four are "regular" numbers and they typically range from 40-60.


What "a04d.att" looks like in Notepad

Each line in the file refers to one image. So line 1 refers to the first image, in which the Creature is facing right, head angled downward. Line 2 refers to the second image, where the head is angled up a little more, line 3 refers to the third image, and so on, in exactly the same order as the sprites in the matching .c16 file.

For both heads and bodies, "duplicates" share coordinates with the first sprite of the same angle and direction. So while a head sprite file has 191 sprites, the corresponding ATT has only sixteen lines. This is because, if the angle and direction are the same between sprites, the coordinates should be the same, regardless of what expression is on a Norn's face or whether their eyes are open or closed. The same is true for body sprites; becoming pregnant does not change where a Norn's arms, legs, head or tail attach to her body.

So a single line refers to a sprite (or group of sprites) representing one angle and direction. The numbers on the line appear to consist of one long string, but they are in fact meant to be read in pairs. ATTs use the cartesian coordinate system to locate joints, and each pair of numbers represents the X (horizontal) and Y (vertical) positions of a joint.

So that first pair of numbers on the first line of the file we've opened ("43 61"), refers to a point which is 43 pixels to the right and 61 pixels down from the top left corner of the sprite.


Plotting the location of a joint on a sprite, using sprite #0 from "a04d.c16" as an example.

Finding the coordinates yourself

In the first image above, we see a sprite with arrows around it indicating the directions of the X and Y coordinates. The first pixel in the upper left corner is considered "zero", and the number increases as you move in the direction that the arrow is pointing. The horizontal value (X) is listed before the vertical (Y), which is typical in Creatures.

Now look at the second image. The horizontal coordinate for this sprite is 43, so I have counted 43 pixels moving right from the left side of the image, and placed a reference point with an arrow to show you exactly where that is.

In the third image I have used the same method to determine the vertical distance from the top. For this sprite/line, the distance is 61 pixels. I have moved the reference point down to that location. You can now see that this pair of coordinates marks the neck joint at the base of the Norn's skull.


The .att file displayed in Notepad (left), shown alongside the corresponding sprites in SpriteBuilder (right).
 Red arrows indicate the first four lines of coordinates and the individual sprites each line corresponds to.
 Number pairs that have been circled in yellow represent the location of the neck joint on the matching sprite.
 Number pairs that have been circled in blue represent the location of the Norn's mouth on the matching sprite.
 Blue and yellow dots mark the locations of the neck and mouth as determined by the encircled numbers.

Naturally, joint coordinates are listed in the same order on each line. So we know the first pair of numbers on every line marks the neck joint in the corresponding image. The second pair of numbers marks the mouth. The third, fourth and fifth pairs are unused in C3/DS. These are where the coordinates for ear and hair attachments appear in Creatures Village Norns; Creatures 3 and Docking Station inherited the engine used by Creatures Adventures, and there are still a few "vestigial" features like this that can be found in various parts of the game.


The layout of ATTs

I have mapped the coordinates within the .att files for an entire Norn, and this is the data that was gleaned from doing so. Here is the key to what all those funky numbers mean!

Just like the sprites, the .atts for all body parts follow a sequence:
 Lines 1-4 correspond to sprites facing right, at all four angles
 Lines 5-8 correspond to sprites facing left, at all four angles
 Lines 9-12 correspond to sprites facing front, at all four angles
 Lines 13-16 correspond to sprites facing back, at all four angles


Listed below is the order that the joints are represented on each line. Note that, with the exception of the body, the joints for a part are listed in order from closest to furthest from the body, ie. hip then knee on the thigh, knee then ankle on the shin, ankle then toe on the foot, and so on.

Head [a---]
 Pair 1 indicates the neck
 Pair 2 indicates the mouth
 Pair 3 is unused
 Pair 4 is unused
 Pair 5 is unused

Body [b---]
 Pair 1 indicates the neck
 Pair 2 indicates the left hip
 Pair 3 indicates the right hip
 Pair 4 indicates the left shoulder
 Pair 5 indicates the right shoulder
 Pair 6 indicates the tail

Thighs [left and right: c--- and f---]
 Pair 1 indicates the hip
 Pair 2 indicates the knee

Shins [left and right: d--- and g---]
 Pair 1 indicates the knee
 Pair 2 indicates the ankle

Feet [left and right: e--- and h---]
 Pair 1 indicates the ankle
 Pair 2 indicates the toe


Humeri [left and right: i--- and k---]
 Pair 1 indicates the shoulder
 Pair 2 indicates the elbow

Radii [left and right: j--- and l---]
 Pair 1 indicates the elbow
 Pair 2 indicates the hand

Tail root [m---]
 Pair 1 indicates the tail base
 Pair 2 indicates the tuft base

Tail tip [n---]
 Pair 1 indicates the tuft base
 Pair 2 indicates the tuft tip


Part 2

How I edit ATTs

Now we know how an ATT file is used by the game, how the information inside it is arranged, and what it all means. But at the end of the day, you still want to edit that breed, and all you have to work with—still—is a bunch of numbers.

Unfortunately, because you have to reload C3/DS to make it pick up on any changes you have made to ATTs in the game, you cannot edit an ATT and watch the changes appear on moving Creatures in real-time. You have to shut the game down, make your changes, and then restart.

Do you like messing with numbers without being able to see what you're doing? Do you like having to stop and restart your game all the time? I don't. It gets old after a while.

I've come up with a strategy to see how a Norn looks and figure out what the coordinates should be without having to load the game. And as long as I'm careful to remember which coordinate pairs correspond to which joints, it works pretty well. I use Photoshop for this, but I'm sure other programs will work too.


Loading sprites

The first thing I do is open up Photoshop and create a new document—I usually make it larger than I need to, just so I feel like I have more "elbow room", but I'm keeping it small this time since I'm posting screenshots here. Once I have my new document, I copy the sprites for the body part I'll be editing from SpriteBuilder and paste them in. Photoshop automatically adds each body part on a new layer. This is good—you're going to want them to all be on different layers for what we'll be doing later.



For this example, I'll be using the body and tail sprites for the male Yautja Norn ("b04v" and "m04v"). Even though I already finished this breed, I'm going to pretend that these are brand-new tail sprites that I just finished cleaning up and now I need to determine the joint coordinates. Because this Norn's anatomy is fairly novel—he's more heavyset and his tail shape is totally different from other C3/DS Norns—I cannot rely on pre-existing ATTs from other breeds, but will have to create them from scratch.

As you can see from the screenshot, I've copied and pasted one of the body sprites into the Photoshop document. This is the sprite I'm going to use to determine where the tail should attach. Since I'm only doing this for tutorial purposes, I'm only going to go over the coordinates for the sprites facing one direction, and I'm only going to use one angle for the body. There isn't much need to show it all, because the same concept will work for any angle.



Now that I have all the sprites I need in the Photoshop file, I select the black background on each with the Magic Wand Tool and go over it with an eraser brush set at 90%, leaving it almost—but not entirely—transparent. This way I will be able to see what's behind the sprites, but I can still easily tell what their original dimensions are.

I also locate the pre-existing tail base joint on the body using the method I described earlier, and mark it with a bright purple dot. The dot is on a separate layer of its own above all the others, so I can make sure it's always in the foreground.


Aligning the sprites

The next step is to arrange the tail sprites on top of one another so that the base is in relatively the same position. This is why they need to be on different layers, and why it helps so much for the sprite backgrounds to be transparent.



It's always good to line up the base of a given part across frames, because you want a Creature's movements to appear consistent, and people will probably notice if a Norn's body parts wobble like they're about to fall off.

Now that we know how the tails line up against each other it's time to determine where they should line up in relation to the body. We select all the tail layers, make sure that only one of them is actually visible, and drag them down to the body until they're positioned in a reasonable location. I made sure the body layer was on top of the tail layers for this part, so it looks about the same as it would in-game.



Then I turn each tail layer off and on, checking them one at a time to make sure they all look good against the body. If one looks a little off, I nudge it until it looks right. Below you can see a quick animation of the different layers as they appear alongside the body.



Once I'm happy with how they look, I turn the body layer off so it's invisible. Now what I'm left with is a bunch of tail sprites and that purple dot. The dot now marks exactly where the tail attaches to the body, so I have the location my coordinates will point to. Obviously for a normal Norn, the tail has a second joint, where the root and the tuft connect. But Yautja Norns only have one tail segment, and I'm sure you get the gist of how this strategy works by now.

All that's left to do is determine the coordinates of that purple dot for each tail sprite and write them down.

Oh no, you're probably thinking—more pixel counting?

Options may vary depending on what program you use, but Photoshop offers a shortcut around this rather tedious step, and I take advantage of it whenever possible.



First make sure you're working on the same layer as the tail sprite you want to measure. Then take the Rectangular Marquee Tool, place it over the purple dot, and drag it up and out, selecting both the purple dot and all of the image above and to the left of it (I usually zoom in first for the sake of accuracy, but thought it would be easier for you to tell what I'm doing this way).

What does this do? Well, Photoshop doesn't copy fully transparent (blank) pixels, and if a large part of your selection consists of blank pixels, it will crop them out, selecting only the pixels that actually have something on them. So hit Ctrl-C, or go to the menu at the top and choose Edit -> Copy.

Now hit Ctrl-N, or go to the menu at the top and choose File -> New. A window will pop up showing the parameters of the new document you're about to create. But wait! We don't want to create a new document, we just want to look at the parameters it's given us.



Look at the two parameters within the red box: Width and Height. These are the dimensions (in pixels, as you can see to the right) for the new document. Where did they come from? Well, Photoshop automatically sets the dimensions of a new document to match the dimensions of whatever is currently in your clipboard (see the Preset drop-down menu above, which reads "Clipboard"). In this case, the clipboard currently contains the chunk of sprite we just selected.

Effectively, we just got Photoshop to measure the distance for us. We now have the exact X (width) and Y (height) coordinates for that tail base joint, and all we had to do to get them was a couple of keystrokes. At this point, we can just type those numbers right into the ATT file where they belong, then repeat the process with the remaining tail sprites.

There isn't much more to it than that.


Conclusion

With this information, you should now be able to edit .att files for new breeds, or fix problems with existing breeds. As always, happy coding!
