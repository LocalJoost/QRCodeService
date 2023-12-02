# QRCodeService
Show an MRTK3 service to read (and position) QR codes using HoloLens 2
Upgraded for latest version of MRTK3 GA.

Before opening issues:
- please pull this MRKT3 branch first
- Use the same Unity version I used (2021.3.23f1)
- Deploy in a HoloLens 2 using the _Master / ARM64_ configuration. _Nothing else works_

![image](https://github.com/LocalJoost/QRCodeService/assets/4129183/40002624-ffbf-4e59-abb5-b486c69eb97e)
- Use Minimum target platform 10.0.18362.0

_Maybe_ other versions work, these settings works _for sure_.

As far as I know:
- Deploying for anything else than Master / ARM64 either doesn't work at all, or is too slow to be useful
- It does not work in the editor, and it doesn't work with Holographic Remoting either.
- If you use different Unity or SDK versions, you are on your own. 

It works. It _really_ works. All of it. 
