# VideoTomb: Video obfuscator

VideoTomb obfuscates videos so humans won't be able to see the content, and they'll probably pass video restriction detections as well.

For an example output video, see Sample.mp4 in the Sample directory. The buck bunny video is used as input. As you'll see the mask image is the only thing distinguishable from the output Sample video, while underlying is the buck bunny video.

The videos can be watched with the HTML/JS decoder in VideoTomb/player directory, which will embed the video player of the upload site in an iframe (can be Youtube), with the encoding mask on top.

Mono support is unavailable due to use of MaterialSkin which is a bit of a shame.

I tried to clean up the directories and files a bit, but am no longer actively working on the project.

Perhaps I'll pick up work later on.

There is an option to encode sound with noise cancellation. The way this works is that left and right channels have random noise added and only output the video sound when joined together when enabling mono sound on Windows or Android. Works wonderfully but would be easily detected by video restriction detection.

The encoder has trouble with longer videos which I have been unable to solve.
