# BLK files

BLK files are basically S16 files with a slightly different header, and with slightly incorrect offsets due to the different header. They contain 128x128 sprites which are the 'blocks' which make up the background.

## The header is as follows (all values are little-endian)

flags 32-bit integer only the first bit is used; if set, the image is in 555 format, if not, the image is in 565 format
backgroundwidth 16-bit integer the width of the background, in blocks (not pixels!)
backgroundheight 16-bit integer the height of the background, in blocks (not pixels!)
numframes 16-bit integer the number of sprites in the file (must always equal backgroundwidth * backgroundheight)

## Then, for each sprite

offset 32-bit integer offset of the sprite data from the start of the file, minus 4.
width 16-bit integer width of the sprite. must be 128.
height 16-bit integer height of the sprite. must be 128.

The rest of the file consists of image data at the file offsets defined in the header. Sprites are stored as an array of horizontal scanlines of the sprite. Each pixel is represented as a 16-bit little-endian integer in the format specified in the header.

The sprites are sorted first horizontally, then vertically (so in an image which was 8 sprites (ie, 8*128 = 1024 pixels) wide and 2 sprites high, you'd get 8 sprites representing the first row, and then 8 sprites representing the second row).
