xamarin_razor_svg
=================

**A proof of concept of using Razor templates, Javascript, and Android WebView to render a SVG chart.**

*Objective*
The goal of this POC was to find out how SVG performs in Android's WebView. A potential benefit of using SVG is to be able to reuse your SVG code for both iOS and Android. If we were to use the native SDKs to draw our charts we may find ourselves having to rewrite the code for each platform. (Granted we could potentially do something with OpenGL, but that's a bit outside of my knowledge base as of now.)

*Implementation*
To figure out the performance, I created a simple interface that consists of three sliders and a Start/Stop Button. The first two sliders modify the current value of each graph. The third slider changes the frame rate/update speed.

*Findings*
I found that trying to reset the whole HTML document for the WebView per frame was not performant at all (not much of a surprise.) Using a simple Javascript function call to update the path data attributes worked much better in terms of performance. I also found that switching screen orientation may cause new instances of the WebView to be generated, thus hurting performance. For this POC, I decided to limit the orientation to portrait only. 

Also, I found this app runs horribly in the emulator but very well on my actual Nexus 7 device.

Another tweak I added was to run all of the string generation code on the Timer thread instead of on the MainUIThread. I had received a couple of warnings that the MainUIThread was trying to do too much work. 

