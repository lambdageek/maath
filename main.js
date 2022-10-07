// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './dotnet.js'

function setText(text) {
    document.getElementById('out').innerHTML = text;
}

function updateTick(text) {
    document.getElementById('tick').innerHTML = text;
}

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create();

setModuleImports('main.js', {
    "MyClass": {
        updateTick,
        setText,
    }
});

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

document.getElementById("startstop").onclick = () => {
    const n = parseInt(document.getElementById("inputN").value);
    exports.MyClass.DoMath(n);
}

exports.MyClass.Tick();

await dotnet.run();
