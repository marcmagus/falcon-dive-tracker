# Auto-Tracking

Automatic tracking is accomplished via a plugin loaded by BepInEx and communicates via UAT.

## Installation

Download the autotracker zip from Releases and unpack it into `path/to/FINAL FANTASY IV PR/BepInEx/plugins/`.

## Usage

Turn on UAT in PopTracker (click UAT in the menu bar) if disabled.

## Limitations

The Guards in Baron Inn do not have an independent flag in the game as you go immediately into the Yang fight. Defeating the fight at Yang will set both locations.

Similarly, Calcobrena is not tracked independently from Golbez.

The CPU fight will not report as defeated. There is an experimental patch for Falcon Dive at `autotracker/fix-cpu-flag.patch` which you can try. It changes which flag is set in the script after fighting CPU. USE AT YOUR OWN RISK.

## Support

If you have trouble you might get some helpful information from `path/to/FINAL FANTASY IV PR/BepInEx/LogOutput.log` or by running PopTracker in a console so you can see its console output.

Feel free to contact me via Discord DM if you are in the Falcon Dive or PopTracker Discords.
