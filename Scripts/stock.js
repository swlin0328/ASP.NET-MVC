
//----------------------------------------------------------------------------------------------

function drawCandle(CanvasId, DataLowest, DataHighest, DataCount) {

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    //可以開始畫圖了:)
    context.strokeStyle = 'gray';
    context.lineWidth = 2;
    context.lineCap = "square";

    var lowest = document.getElementById(DataLowest).innerText;
    var highest = document.getElementById(DataHighest).innerText;
    var Count = document.getElementById(DataCount).innerText;

    var widthInterval = Math.round(600 / (parseInt(Count) + 1));
    var hightInterval = Math.round(400 / (highest - lowest));

    for (var i = 0; i < Count; i++) {
        
        var Price_Low = document.getElementById('Low_' + i.toString()).innerText;
        var Price_High = document.getElementById('High_' + i.toString()).innerText;
        var Price_Open = document.getElementById('Open_' + i.toString()).innerText;
        var Price_Close = document.getElementById('Close_' + i.toString()).innerText;

        var x1 = Math.round(600 - widthInterval * (i + 1));
        var x2 = Math.round(600 - widthInterval * (i + 1));
        var y1 = Math.round(425 - hightInterval * (Price_Low - lowest));
        var y2 = Math.round(425 - hightInterval * (Price_High - lowest));

        if (Price_Open > Price_Close) {
            context.strokeStyle = 'green';
        }
        else if (Price_Open < Price_Close) {
            context.strokeStyle = 'red';
        }
        else {
            context.strokeStyle = 'black';
        }
        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawMA(CanvasId, DataLowest, DataHighest, DataCount) {
    
    var lowest = document.getElementById(DataLowest).innerText;
    var highest = document.getElementById(DataHighest).innerText;
    var Count = document.getElementById(DataCount).innerText;

    var widthInterval = Math.round(600 / (parseInt(Count) + 1));
    var hightInterval = Math.round(400 / (highest - lowest));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");

    //可以開始畫圖了:)
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'purple';
    for (var i = 0; i < 69; i++) {
        var x1 = Math.round(600 - widthInterval * (i + 1));
        var x2 = Math.round(600 - widthInterval * (i + 2));

        var y1 = document.getElementById('MA20_' + i.toString()).innerText;
        var y2 = document.getElementById('MA20_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - hightInterval * (y1 - lowest));
        y2 = Math.round(425 - hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }

    context.strokeStyle = 'blue';
    for (var i = 0; i < 49; i++) {
        var x1 = Math.round(600 - widthInterval * (i + 1));
        var x2 = Math.round(600 - widthInterval * (i + 2));

        var y1 = document.getElementById('MA40_' + i.toString()).innerText;
        var y2 = document.getElementById('MA40_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - hightInterval * (y1 - lowest));
        y2 = Math.round(425 - hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}
//----------------------------------------------------------------------------------------------

function drawCYQ(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'purple';
    for (var i = 0; i < 14; i++) {
        var x1 = Math.round(600 - 5 * widthInterval * i);
        var x2 = Math.round(600 - 5 * widthInterval * (i + 1));

        var y1 = document.getElementById('CYQ_' + i.toString()).innerText;
        var y2 = document.getElementById('CYQ_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - hightInterval * (y1 - lowest));
        y2 = Math.round(425 - hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawVolume(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'blue';
    for (var i = 0; i < 59; i++) {
        var x1 = Math.round(600 - widthInterval * (i + 1));
        var x2 = Math.round(600 - widthInterval * (i + 2));

        var y1 = document.getElementById('VolumeRate_' + i.toString()).innerText;
        var y2 = document.getElementById('VolumeRate_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - hightInterval * (y1 - lowest));
        y2 = Math.round(425 - hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawM1B(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'orange';
    for (var i = 0; i < 5; i++) {
        var x1 = Math.round(600 - 20 * widthInterval * i);
        var x2 = Math.round(600 - 20 * widthInterval * (i + 1));

        var y1 = document.getElementById('MonetaryM1B_' + i.toString()).innerText;
        var y2 = document.getElementById('MonetaryM1B_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - 5 * hightInterval * (y1 - lowest));
        y2 = Math.round(425 - 5 * hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawM2(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'red';
    for (var i = 0; i < 5; i++) {
        var x1 = Math.round(600 - 20 * widthInterval * i);
        var x2 = Math.round(600 - 20 * widthInterval * (i + 1));

        var y1 = document.getElementById('MonetaryM2_' + i.toString()).innerText;
        var y2 = document.getElementById('MonetaryM2_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - 5 * hightInterval * (y1 - lowest));
        y2 = Math.round(425 - 5 * hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawLibor(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'green';
    for (var i = 0; i < 99; i++) {
        var x1 = Math.round(600 - widthInterval * i);
        var x2 = Math.round(600 - widthInterval * (i + 1));

        var y1 = document.getElementById('Libor_' + i.toString()).innerText;
        var y2 = document.getElementById('Libor_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - 20 * hightInterval * (y1 - lowest));
        y2 = Math.round(425 - 20 * hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawFinacing(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'brown';
    for (var i = 0; i < 29; i++) {
        var x1 = Math.round(600 - widthInterval * i);
        var x2 = Math.round(600 - widthInterval * (i + 1));

        var y1 = document.getElementById('Voucher_' + i.toString()).innerText;
        var y2 = document.getElementById('Voucher_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - hightInterval * (y1 - lowest));
        y2 = Math.round(425 - hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawBDI(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'black';
    for (var i = 0; i < 19; i++) {
        var x1 = Math.round(600 - widthInterval * i);
        var x2 = Math.round(600 - widthInterval * (i + 1));

        var y1 = document.getElementById('BDI_' + i.toString()).innerText;
        var y2 = document.getElementById('BDI_' + (i + 1).toString()).innerText;

        y1 = y1 * 0.052;
        y2 = y2 * 0.052;

        y1 = Math.round(425 - hightInterval * (y1 - lowest));
        y2 = Math.round(425 - hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawCRB(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'magenta';
    for (var i = 0; i < 19; i++) {
        var x1 = Math.round(600 - widthInterval * i);
        var x2 = Math.round(600 - widthInterval * (i + 1));

        var y1 = document.getElementById('CRB_' + i.toString()).innerText;
        var y2 = document.getElementById('CRB_' + (i + 1).toString()).innerText;

        y1 = (y1 - 150) * 1.5;
        y2 = (y2 - 150) * 1.5;
        
        y1 = Math.round(425 - hightInterval * (y1 - lowest));
        y2 = Math.round(425 - hightInterval * (y2 - lowest));
        
        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function drawOil(CanvasId, DataCount) {

    var hightInterval = Math.round(400 / 80);
    var lowest = 0;

    var Count = document.getElementById(DataCount).innerText;
    var widthInterval = Math.round(600 / (parseInt(Count) + 1));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'darkolivegreen';
    for (var i = 0; i < 19; i++) {
        var x1 = Math.round(600 - widthInterval * i);
        var x2 = Math.round(600 - widthInterval * (i + 1));

        var y1 = document.getElementById('Oil_' + i.toString()).innerText;
        var y2 = document.getElementById('Oil_' + (i + 1).toString()).innerText;

        y1 = Math.round(425 - hightInterval * (y1 - lowest));
        y2 = Math.round(425 - hightInterval * (y2 - lowest));

        context.beginPath();
        context.moveTo(x1, y1);
        context.lineTo(x2, y2);
        context.stroke();
    }
}

//----------------------------------------------------------------------------------------------

function clearCanvas(CanvasID) {
    var canvas = document.getElementById(CanvasID);
    var context = canvas.getContext("2d");

    //可以開始畫圖了:)
    context.strokeStyle = 'gray';
    context.lineWidth = 2;
    context.lineCap = "square";
    context.clearRect(0, 0, canvas.width, canvas.height);

    context.beginPath();
    context.moveTo(0, 225);
    context.lineTo(600, 225);
    context.stroke();
}

//----------------------------------------------------------------------------------------------

function checkValue(id, Value, Standard) {
    if (Value > Standard)
    {
        document.getElementById(id).className = "green";
    }
}

//----------------------------------------------------------------------------------------------

function OpenClose(id) {
    if (document.getElementById(id).innerText == "Close ↑")
    {
        var show = document.getElementById(id);
        show.innerHTML = "<a href='javascript: void (0)' class='blue'>Open ↓</a>";

        var txtContent = next(show);
        txtContent.style.display = 'none';
    }
    else
    {
        var show = document.getElementById(id);
        show.innerHTML = "<a href='javascript: void (0)' class='blue'>Close ↑</a>";

        var txtContent = next(show);
        txtContent.style.display = 'block';
    }
}

//----------------------------------------------------------------------------------------------

function drawBBands(CanvasId, DataLowest, DataHighest, DataCount) {

    var lowest = document.getElementById(DataLowest).innerText;
    var highest = document.getElementById(DataHighest).innerText;
    var Count = document.getElementById(DataCount).innerText;

    var widthInterval = Math.round(600 / (parseInt(Count) + 1));
    var hightInterval = Math.round(400 / (highest - lowest));

    var canvas = document.getElementById(CanvasId);
    var context = canvas.getContext("2d");

    //可以開始畫圖了:)
    context.lineWidth = 2;
    context.lineCap = "square";

    context.strokeStyle = 'brown';

    var sigma = new Array();
    var num = 20;

    for (var i = 0; i < 70; i++)
    {
        var temp = 0;
        var u = 0;
        u = document.getElementById('MA20_' + i.toString()).innerText;

        for (var j = 0; j < num; j++)
        {       
            var x;
            x = document.getElementById('Close_' + (i+j).toString()).innerText;
            temp += (x - u) * (x - u);
        }

        temp = Math.sqrt(temp / num);
        sigma.push(temp);
    }

    for (var j = 0; j < 2; j++) {
        for (var i = 0; i < 69; i++) {
            var x1 = Math.round(600 - widthInterval * (i + 1));
            var x2 = Math.round(600 - widthInterval * (i + 2));

            var y1 = document.getElementById('MA20_' + i.toString()).innerText;
            var y2 = document.getElementById('MA20_' + (i + 1).toString()).innerText;

            y1 = y1 - lowest;
            y2 = y2 - lowest;

            y1 = Math.round(425 - hightInterval * (y1 + (1.5 - 3 * j) * sigma[i]));
            y2 = Math.round(425 - hightInterval * (y2 + (1.5 - 3 * j) * sigma[i + 1]));


            context.beginPath();
            context.moveTo(x1, y1);
            context.lineTo(x2, y2);
            context.stroke();
        }
    }
}

//----------------------------------------------------------------------------------------------

function calVolatility() {

    var TR = new Array();
    var N = 0;
    var price = document.getElementById('Close_0').innerText;
    var num = 20;
    
    for (var i = 0; i < num; i++)
    {
        var Price_Low = document.getElementById('Low_' + i.toString()).innerText;
        var Price_High = document.getElementById('High_' + i.toString()).innerText;
        var Price_Close = document.getElementById('Close_' + (i + 1).toString()).innerText;
        
        var H_L, H_PDC, PDC_L, max;

        H_L = Price_High - Price_Low;
        H_PDC = Price_High - Price_Close;
        PDC_L = Price_Close - Price_Low;

        if (H_L > H_PDC && H_L > PDC_L)
        {
            max = H_L;
        }
        else if (H_PDC > PDC_L)
        {
            max = H_PDC;
        }
        else
        {
            max = PDC_L;
        }
        TR.push(max);
        
    }
    
    for (var j = 0; j < num; j++)
    {
        N += TR[j];
    }

    N = (N / 20) * 100 / price;
    document.getElementById('Volatility').innerText = "價格波動率%：" + N.toFixed(2);
}

//----------------------------------------------------------------------------------------------

function calEconomy() {

    var BDI, Oil;

    var BDI_Recent = document.getElementById('BDI_0').innerText;
    var BDI_Last = document.getElementById('BDI_19').innerText;
    var Oil_Recent = document.getElementById('Oil_0').innerText;
    var Oil_Last = document.getElementById('Oil_19').innerText;

    Oil = 100 * (Oil_Recent - Oil_Last) / Oil_Last;
    BDI = 100 * (BDI_Recent - BDI_Last) / BDI_Last;

    document.getElementById('BDI').innerText = "BDI月漲幅%：" + BDI.toFixed(2);
    document.getElementById('Oil').innerText = "油價月漲幅%：" + Oil.toFixed(2);
}

//----------------------------------------------------------------------------------------------
function next(elem) {
    do {
        elem = elem.nextSibling;
    } while (elem && elem.nodeType !== 1);
    return elem;
}