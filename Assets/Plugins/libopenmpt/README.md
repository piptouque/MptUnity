

## Importing libopenmpt for MptSharp

### IMPORTANT

### see: https://forum.unity.com/threads/unity-dllnotfoundexception-when-adding-so-plugins.379721/

### What was done:

#### All platforms

- Put shared libraries in `Assets/Plugins`.

#### Linux

- Renamed `libopenmpt.so` to `liblibopenmpt.so`. This looks stupid, but with Linux Unity looks for the library MyLibrary under libMyLibrary.so.

#### MacOs

- Todo!
