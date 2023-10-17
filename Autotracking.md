# Auto-Tracking

Automatic tracking is accomplished via a perl script which monitors the Pixel Remaster Auto-Save functionality. Pixel Remaster updates your auto-save every time you map transition, and the script observes the file modification and parses it for relevant data. It communicates this to PopTracker via websockets using UATBridge/UAT.

## Dependencies

* Anub1sR0cks' [FFPRSaveEditor](https://github.com/Anub1sR0cks/FFPRSaveEditor)

    We use the Decrypt program to convert the savefile into plaintext JSON.

    On Windows you can probably download the release; on Linux I had to build from source. Place the Decrypt executable or a symlink to it in `autotracker/`.

* [UATBridge](https://github.com/black-sliver/UATBridge)

    You will need to build from source. Place anywhere convenient.

* Perl

    You will need to set up a working perl environment including library dependencies, which can be either globally or locally installed. The script checks for libraries under `autotracker/lib/perl5/` for your convenience.

    Dependencies I know aren't included in core perl are:
  * [AnyEvent::Websocket::Client](https://metacpan.org/pod/AnyEvent::WebSocket::Client)
  * [AnyEvent::Filesys::Notify](https://metacpan.org/pod/AnyEvent::Filesys::Notify)
  * [JSON::XS](https://metacpan.org/pod/JSON::XS)

## Usage

At any point before or after the below, run Falcon Dive, start FF4PR, start PopTracker.

Locate your auto-save file in the Final Fantasy IV PR saves directory. It will probably be the most recently used large file there. In my experience the file name for this slot never changes, so you should only need to locate it once and remember the name.

Run UATBridge. It should start listening for connections.

Run `autotracker/ff4pr-autotracker AUTOSAVEFILE`. It should scan your last auto-save and connect to UATBridge.

Turn on UAT in PopTracker (click UAT in the menu bar) if disabled. It should connect to UATBridge and update everything to match the last state.

## Limitations

Pixel Remaster updates the auto-save on map transition. As a result, your tracker will usually not report boss defeat or item receipt immediately. It will update the next time you go to a new map. This is particularly noticeable in locations with multiple fights in a row.

The Guards in Baron Inn do not have an independent flag in the game as you go immediately into the Yang fight. Defeating the fight at Yang will set both locations.

Similarly, Calcobrena is not tracked independently from Golbez.

## Support

Feel free to contact me via Discord DM if you are in the Falcon Dive or PopTracker Discords.

Please understand that I am on Linux. If you are on Windows my ability to provide support will be extremely limited. There may be flaws in my attempt to make the script portable to Windows.
