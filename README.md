Tethys.Silverlight
==================

Basic Services and Resources Library for Silverlight, WPF, Windows Phone, etc.

## Solution Overview ##

* Tethys.Silverlight.SL4 - library version for Slilverlight 4
* Tethys.Silverlight.SL5 - library version for Slilverlight 5
* Tethys.Silverlight.WinRt - library version for Windows RT apps
* Tethys.Silverlight.WP8 - library version for Windows Phone 8
* Tethys.Silverlight.WP71 - library version for Windows Phone 71
* Tethys.Silverlight.WPF - library version for WPF

## Main Features ##

* Base classes for MVVM
* Cryptographic methods (because they were not available on all platforms)
* A lot of converters
* A lot of behaviors
* Some helper class for controls  

## Build ##

### Requisites ###

* Visual Studio 2013

### Symbols for conditional compilation ###
* WINDOWS       ==> Windows platform
* NETFX_CORE    ==> Windows 8 / WinRT / Metro applications
* WINDOWS_PHONE ==> Windows Phone platform
* SILVERLIGHT   ==> Silverlight platofrm
* NET45         ==> .Net version 4.5 and later

## Thanks ##

Not all code is from me. Code parts are from:

* Peter Torr (TiltEffect)
* James McCaffrey (AES)
* David Kerr's Apex framework ([http://apex.codeplex.com/](http://apex.codeplex.com/)): EasyGrid, GridLengthConverter, DependencyObjectExtensions, FrameworkElementExtensions, SafeObservableCollection, AssembliesHelper

## Copyright & License ##

Copyright 2010-2015 T. Graf

Licensed under the **Apache License, Version 2.0** (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and limitations under the License.

Code from Apex framework is MIT licensed:

**The MIT License (MIT)**

Copyright (c) 2011 David Kerr

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
