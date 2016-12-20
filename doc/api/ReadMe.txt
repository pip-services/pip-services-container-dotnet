After regeneration of help documentation replace script in Index.html with this one:

        window.location.replace"html/html/8c1c4534-fcf8-4adb-81a6-b39ad99b4404.htm");

with this one:

        var base = window.location.href;
        base = base.substr(0, base.lastIndexOf("/") + 1);
        window.location.replace(base + "html/8c1c4534-fcf8-4adb-81a6-b39ad99b4404.htm");
