Known (non)Issues:

Issue: When attempting to run or debug after building from source, a strong-name exception will be encountered.
Fix: Open an administrative command prompt and execute: "sn.exe -Vr *,aafe07b9ddf0a2a4" to add a verification skip entry.