# ![Minecraft Orchestrator](assets/logo.png) MineOrc

![CalVer](https://img.shields.io/badge/calver-YYYY.MINOR.MICRO-22bfda?style=flat-square)

MineOrc (*Mine*craft *Orc*hestrator) is a Minecraft instance manager and
launcher that works from the command line.

## Features

- [x] Game launch
- [x] Profile/Instance management
- [x] Java registration & management
- [ ] Microsoft Authentication (MSA)
- [ ] Mod loaders support

## Running

MineOrc requires .NET 10 Runtime. You can get it [here](https://dot.net/).

To get a list of commands, run `mineorc --help`.

## Building & contributing

To build, you will need .NET 10. You can download it [here](https://dot.net).

Note that in order to build you will need to specify a secrets file. It should
be located under:

> [!IMPORTANT]
> In order for the app to build and run correctly, you will need to specify a
> secrets file. It should be located under `MineOrc/Resources/Secrets.json`,
> and it must be conforming to
> [this class file](./MineOrc/Resources/SecretModel.cs).
>
> If you don't have the API keys, just leave the value blank.

### Developing

To develop, you probably need a decent .NET IDE. Your free options are:

- [Visual Studio](https://visualstudio.com) Community (non-commerical or
  startup use)
- [Visual Studio Code](https://code.visualstudio.com), with C# Dev Kit (same as
  Visual Studio)
- [Rider](https://jetbrains.com/rider) (non-commercial use)
- Emacs, etc. editors that support Language Server Protocol

To contribute, fork, modify and open a pull request. To report a bug or request
a feature, please use Issues.

## Legal

This project is licensed under the GPL-3.0-or-later licence.

**Disclaimer**: An AI assistant was used when developing this computer
program. For now, every AI output that made into the code are in-line typing
assistance. All AI output are human reviewed and resembles what the developer
would have coded manually by hand.

<!-- SPDX-FileCopyrightText: 2025 WithLithum & contributors -->
<!-- SPDX-License-Identifier: GPL-3.0-or-later -->