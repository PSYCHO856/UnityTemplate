## How to take screenshot
1. Press F12 to open Smart Screenshot Taker window.
2. Select aspect ratios you want to shoot.
3. Change “Extra size” value if you want to take screenshot with multiplied size.
4. Press “Take Screenshot” or F12 to take screenshot.
5. Screenshots will be saved in “Screenshots” folder in base project folder.

## How add custom resolution
1. Open  SmartScreenshotTaker\Editor\AspectRatios.cs
2. Change existing ratios or add new value where first parameter is name, second -
   width, third - height.

## How open fullscreen window
1. Select any editor window.
2. Press F9 to open fullscreen selected window
3. Press again F9 to close opened window.

## How to recalculate UI before screenshot
1. Add  ScreenshotResize attribute to your function.
2. This function will be called before every screenshot.
3. Result:
