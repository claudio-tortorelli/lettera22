# lettera22
Lettera22 is a toolset for automated text processing that transforms raw text files into stylish artifacts (eg HTML).

It consists of a series of command line tools (CLI), each of which has a specific role in the processing and management 
of published texts. A portable version of the well-known Notepad ++ (npp) editor is integrated in the package, 
to support the text writing and contextual start of the tools.

Actually no sources are provided but you can download and test current binary version.
Sorry but no translated documentation is currently ready: the tool is meant for Italian writers ;)
I supplied an "hello world" in the Wiki to give a basic overview of the software.

Links
-----
- Download page: https://www.claudiotortorelli.it/lettera22-download-software-last-version-changelog.html
- Main guide (italian): https://www.claudiotortorelli.it/lettera22-guida-per-lo-scrittore.html#2
- FAQ: https://www.claudiotortorelli.it/lettera22-faq-informazioni-software.html
- Demo page: https://www.claudiotortorelli.it/lettera22-test-tutte-le-funzioni-stile.html

Installation info
-----
Unpack the archive in a local folder and start "run.bat". It is possible to copy the folder on a USB stick: the software does not use external references to its own folder.

License
------
Lettera22 is distributed with MIT license (https://choosealicense.com/licenses/mit/).

Features
-----
- parser --> compiler --> linker document flow
- simplified but powerfull syntax to define document structure and elements
- offline and online work mode
- automatic main page (index) generation
- automatic document revision history
- autoconsistent and portable final document 
- automatic document publishing to remote web space
- db-less, no database needed, no SQL injection
- only basic technologies involved produce safe and durable documents
- support to "PEC" signature, to improve copyright
- predefined and optimized document style
- no advertisement, cookie, profiling or statistics: GDPR ready!
- easy to be integrated into other software
- free of use

Some technical detail
---------------------
Lettera22 is developed in C# with target framework 4.7.2. At the moment it is therefore only for Windows platform (from Windows 7). It is a modular software, or toolset, consisting of a set of console applications  which can be easily integrated into batch scripts or other software. It has a configuration file through which it is possible to customize its behavior (basic and advanced options).

Lettera22 is designed to be self-consistent: it is not installed, but is performed directly within a folder containing the necessary data.

Embedded C# dependencies:
- OpenPop (POP3 mail)
- Aegis Implicit Mail (https://sourceforge.net/projects/netimplicitssl/)
- HtmlAgilityPack.NetCoreCodePages (https://www.nuget.org/packages/HtmlAgilityPack.NetCoreCodePages/)

External dependencies:
- Notepad ++ (https://notepad-plus-plus.org/download/)
- Sorttable.js https://www.kryogenix.org/code/browser/sorttable/)

The first version included integration with IPFS (https://dist.ipfs.io/#go-ipfs), which has however been removed at present.

Why 'Lettera22'?
-----
It is a tribute to Olivetti's Lettera 22, a typewriter that made an epoch for style and practicality. It is still considered an exceptional tool, an emblem of creativity and functionality, celebrated in the museums of technology.
