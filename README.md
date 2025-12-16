# ![MineOrc logo](assets/logo.png) MineOrc

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/WithLithum/MineOrc/cake.yml?style=flat-square&logo=github)
![CalVer](https://img.shields.io/badge/calver-YYYY.MINOR.MICRO-22bfda?style=flat-square)
![License](https://img.shields.io/badge/license-GPL--3.0--or--later-red?style=flat-square)

MineOrc is an instance manager and launcher for Minecraft: Java Edition.

## Features

- [x] Game launch
- [x] Profile/Instance management
- [x] Java registration & management
- [ ] Microsoft Authentication (MSA)
- [ ] Mod loaders support

## Download & Usage

MineOrc requires .NET 10 Runtime. You can get it [here](https://dot.net/).

There are no release builds at the moment, but you can get a CI build from
[here](https://nightly.link/WithLithum/MineOrc/workflows/cake/trunk/App%20binaries).
In any case it is inaccessible because it was throttled by GitHub, login and
use the 'Actions' page to get one.

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

This project is licensed under the GPL-3.0-or-later licence. See the legal
code [here](COPYING.txt).

MineOrc is not an official Minecraft product, and is not associated with nor
approved by Mojang or Microsoft.

<!-- SPDX-FileCopyrightText: 2025 WithLithum & contributors -->
<!-- SPDX-License-Identifier: GPL-3.0-or-later -->