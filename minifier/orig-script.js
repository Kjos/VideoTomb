/*!
* Copyright VideoTomb.com. All rights reserved. Do not copy, do not distribute.
*/
var options = options || {};

options.videoWidth = 1280, options.videoHeight = 720, options.randomLines = !0, 
options.resolution = 21, options.columnWidth = 9, options.vertLines = !0, options.hash = "0", 
options.version = 1, options.videoElement = !0, options.videoUrl = "", options.maskUrl = "";

var funcs = funcs || {}, lineData = [], loc = window.location, hashSkip = 1e6, mask = document.getElementById("mask"), video = document.getElementById("frame"), error = document.getElementById("error"), multiplier = document.getElementById("multiplier"), localFile = document.getElementById("localfile"), imageForm = document.getElementById("imageform"), ctx = mask.getContext("2d"), ctxm = multiplier.getContext("2d");

window.onhashchange = window.onload = function() {
    funcs.init();
}, funcs.init = function() {
    multiplier.width = 1, multiplier.height = 1, window.onresize = funcs.resize, window.focus(), 
    window.onblur = function() {
        setTimeout(function() {
            options.delay ? setTimeout(funcs.showMask, 1e3 * options.delay) : funcs.showMask(), 
            window.focus();
        }, 100);
    }, document.getElementById("fullscr").onclick = funcs.fullScreen, funcs.hideError(), 
    funcs.hideMask();
    try {
        funcs.parseUrl();
    } catch (o) {
        funcs.showError("There was a problem parsing the URL.");
    }
    funcs.copyrightCheck();
}, funcs.showError = function(o) {
    throw error.style.visibility = "visible", video.style.visibility = "hidden", funcs.hideMask(), 
    funcs.hideImageForm(), error.innerHTML = o, 0;
}, funcs.hideError = function() {
    error.style.visibility = "hidden", video.style.visibility = "visible";
}, funcs.showImageForm = function() {
    imageForm.style.visibility = "visible", error.style.visibility = "hidden", video.style.visibility = "hidden", 
    funcs.hideMask();
}, funcs.hideImageForm = function() {
    imageForm.style.visibility = "hidden", video.style.visibility = "visible";
}, funcs.showMask = function() {
    for (var o = 0; o < layers.length; o++) layers[o].style.visibility = "visible";
}, funcs.hideMask = function() {
    for (var o = 0; o < layers.length; o++) layers[o].style.visibility = "hidden";
}, funcs.charCode = function(o) {
    return o.charCodeAt(0);
}, funcs.copyrightCheck = function() {
    loc.href.startsWith("file:") || loc.href.contains("videotomb.com/") || funcs.showError("Warning: VideoTomb decoder is copyright protected.");
}, funcs.fullScreen = function() {
    var o = document.documentElement;
    (o.requestFullScreen || o.webkitRequestFullScreen || o.mozRequestFullScreen || o.msRequestFullscreen).call(o);
}, funcs.decodeInt = function(o) {
    for (var e = 0, i = 1, n = 0; n < o.length; n++, i *= 62) {
        var t = o[n], s = 0;
        isNaN(1 * t) ? t == t.toLowerCase() ? (s = funcs.charCode(t) - funcs.charCode("a"), 
        s += 10) : t == t.toUpperCase() && (s = funcs.charCode(t) - funcs.charCode("A"), 
        s += 36) : s = funcs.charCode(t) - funcs.charCode("0"), e += s * i;
    }
    return e;
}, funcs.unshrinkUrl = function(o, e, i) {
    for (var n = new Array("../", "https://", "http://", "www.", "player.", "i.imgur.com", "twitch.tv", "clips.twitch.tv", "bilibili.com", "vimeo.com", "facebook.com", "flickr.com", "instagram.com"), t = !1, s = 0; s < o.length; s++) {
        var r = funcs.decodeInt(o[s]);
        e = n[r] + e, t |= r > 4;
    }
    i ? (options.videoUrl = e, options.videoElement = !t) : options.maskUrl = e;
}, funcs.parseUrl = function() {
    var o = loc.hash.substring(1).split("&"), e = o[0];
    options.version = funcs.decodeInt(e.substring(0, 1)), options.delay = funcs.decodeInt(e.substring(1, 2));
    var i = o[1], n = i.indexOf("="), t = i.substring(0, n), s = decodeURIComponent(i.substring(n + 1));
    options.videoElement = !0, options.videoUrl = null, funcs.unshrinkUrl(t, s, !0), 
    options.hash = null, options.maskUrl = null;
    try {
        var r = o[2], a = r.indexOf("="), l = r.substring(0, a), c = decodeURIComponent(r.substring(a + 1));
        switch (l) {
          case "hash":
            options.videoWidth = funcs.decodeInt(c.substring(0, 3)) % 16384, options.videoHeight = funcs.decodeInt(c.substring(3, 6)) % 16384, 
            options.columnWidth = funcs.decodeInt(c.substring(6, 8)) % 100, options.resolution = funcs.decodeInt(c.substring(8, 10)) % 128, 
            options.vertLines = funcs.decodeInt(c.substring(10, 11)) % 2 == 1, options.randomLines = funcs.decodeInt(c.substring(11, 12)) % 2 == 1, 
            options.hash = c, console.log("columnWidth: " + options.columnWidth), console.log("vertLines: " + options.vertLines), 
            console.log("randomLines: " + options.randomLines), console.log("hash: " + options.hash);
            break;

          default:
            funcs.unshrinkUrl(l, c, !1);
        }
    } catch (o) {
        return void funcs.showImageForm();
    }
    console.log("version: " + options.version), console.log("resolution: " + options.resolution), 
    console.log("videoWidth: " + options.videoWidth), console.log("videoHeight: " + options.videoHeight), 
    funcs.loadPage();
};

var utils = utils || {};

utils.FNV_OFFSET_32 = 2166136261, utils.hashFnv32a = function(o) {
    for (var e = utils.FNV_OFFSET_32, i = 0; i < o.length; i++) e ^= 255 & o.charCodeAt(i), 
    e += (e << 1) + (e << 4) + (e << 7) + (e << 8) + (e << 24);
    return e >>> 0;
};

for (var layers = document.getElementsByClassName("layer"), i = 0; i < layers.length; i++) layers[i].onclick = function() {
    funcs.hideMask();
};

localfile.onchange = function(o) {
    var e = o.target.files[0], i = new FileReader();
    options.hash = null, options.maskUrl = null, i.onload = function(o) {
        options.maskUrl = o.target.result, funcs.loadPage();
    }, i.readAsDataURL(e);
}, funcs.loadPage = function() {
    options.videoUrl ? options.videoElement ? video.src = "iframe.html#" + options.videoUrl : video.src = options.videoUrl : funcs.showError("The video is missing.");
    try {
        if (options.hash) funcs.generateNoise(); else {
            if (!options.maskUrl) throw 0;
            funcs.loadImage();
        }
    } catch (o) {
        funcs.showError("There was a problem loading the mask.");
    }
    funcs.hideError(), funcs.hideImageForm();
};

var secretKey = "lk309fl";

funcs.loadImage = function() {
    var o = new Image();
    o.onload = function() {
        var e = options.videoWidth = o.width, i = options.videoHeight = o.height;
        mask.width = e, mask.height = i;
        for (var n = ctx.createImageData(e, i), t = n.data, s = 0, r = 0, a = 0, l = Math.floor(e * i / hashSkip), c = 0, u = 0; u < i; u++) for (var d = 0; d < e; d++) {
            for (var h = 0; h < 3; h++, s++) 0 == c && (a = utils.hashFnv32a(secretKey + s + e + i), 
            c = l), t[r + (2 - h)] = a % 2 == 1 ? 255 : 0, c--;
            r += 3, t[r++] = 255;
        }
        ctx.putImageData(n, 0, 0), ctx.globalCompositeOperation = "difference", ctx.drawImage(o, 0, 0), 
        ctxm.drawImage(o, 0, 0), funcs.resize();
    }, o.src = options.maskUrl;
}, funcs.generateNoise = function() {
    var o = options.videoWidth, e = options.videoHeight;
    mask.width = o, mask.height = e;
    for (var i = ctx.createImageData(o, e), n = i.data, t = 0, s = 0, r = 0, a = Math.floor(o * e / hashSkip), l = 0, c = 0; c < e; c++) for (var u = 0; u < o; u++) {
        for (var d = 0; d < 3; d++, t++) 0 == l && (r = utils.hashFnv32a(options.hash + t), 
        l = a), n[s + (2 - d)] = r % 255 & 255;
        s += 3, n[s++] = 255;
    }
    for (var h = 4 * o, f = 0; f < 3; f++) {
        for (c = 0; c < e; c++) for (u = 0; u < o; u++) {
            for (var m = 0, p = c, v = -3; v <= 3; v++) t = p * h + 4 * Math.max(0, Math.min(o - 1, u + v)), 
            m += n[t += f];
            n[c * h + 4 * u + f] = Math.floor(m / 13);
        }
        for (c = 0; c < e; c++) for (u = 0; u < o; u++) {
            m = 0;
            for (var g = -3; g <= 3; g++) p = c + g, t = (p = Math.max(0, Math.min(e - 1, p))) * h + 4 * u, 
            m += n[t += f];
            n[c * h + 4 * u + f] = Math.floor(m / 13);
        }
    }
    (lineData = new Array(3 * Math.max(o, e)))[0] = utils.hashFnv32a(options.hash) % options.resolution - options.resolution / 2, 
    options.randomLines || (lineData[0] = lineData[0] >= 0 ? options.resolution / 2 : -options.resolution / 2);
    for (var y = 0, w = 1; w < lineData.length; w++) if (y > 0) lineData[w] = lineData[w - 1], 
    y--; else {
        if (options.randomLines) {
            var b = utils.hashFnv32a(options.hash + w), k = lineData[w - 1] >= 0 ? -1 : 1;
            lineData[w] = b % options.resolution * k, y = b % options.columnWidth + options.columnWidth;
        } else lineData[w] = -lineData[w - 1], y = options.columnWidth;
        y--;
    }
    var I = 0;
    for (c = 0; c < e; c++) for (u = 0; u < o; u++) {
        for (d = 2; d >= 0; d--, I++) {
            var x, F = n[I];
            F = Math.floor(F * (255 - 2 * options.resolution) / 255) + options.resolution, F += x = options.vertLines ? lineData[3 * u + d] : lineData[3 * c + d], 
            F = x > 0 ? Math.min(255 - options.resolution, F) : Math.max(options.resolution, F), 
            n[I] = F;
        }
        I++;
    }
    ctx.putImageData(i, 0, 0), ctx.imageSmoothingEnabled = !0;
    var E = 255 - options.resolution;
    ctxm.fillStyle = "rgb(" + E + "," + E + "," + E + ")", ctxm.fillRect(0, 0, 1, 1), 
    funcs.resize();
}, funcs.resize = function() {
    var o = options.videoWidth / options.videoHeight, e = window.innerWidth, i = window.innerHeight, n = (i - 80) * o, t = 0;
    n < e && (t = (e - n) / 2, e = n), video.style.left = t + "px", video.style.top = "0px", 
    video.style.width = e + "px", video.style.height = i + "px";
    var s, r, a, l, c, u = o / (e / i);
    u > 1 ? (s = t, r = (i - (c = i / u)) / 2, a = e, l = c) : (s = (e - (c = e * u)) / 2 + t, 
    r = 0, a = c, l = i);
    for (var d = 0; d < layers.length; d++) {
        var h = layers[d];
        h.style.left = s + "px", h.style.top = r + "px", h.style.width = a + "px", h.style.height = l + "px";
    }
};