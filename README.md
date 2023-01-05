# V2rayRunner
auto run "v2ray.exe -c xx.config" at windows startup, silently in background.

# V2rayKiller
kill V2rayRunner.exe as well as v2ray.exe instances.

# how to use
- build your own or download prebuild V2rayKiller.exe and V2rayRunner.exe from https://github.com/decemberpei/V2rayRunner/tree/master/binary
- put V2rayKiller.exe and V2rayRunner.exe into the same dir as v2ray.exe (v2ray config files should also locate in the same dir).
- double click V2rayRunner.exe and v2ray will run against each of the config files. next time you reboot OS, v2ray will auto start.
- to mannually restart v2ray, run V2rayKiller.exe first then run V2rayRunner.exe.

