// Run immediately
soundManager.url = rootPath + 'Content/SoundManager2/';

// DOM Ready
$(function () {
    var currentContext = { CurrentController: "Terminal", CurrentPage: 1, Enabled: false, Forced: false, ScrollToBottom: true };
    ResizeCLI();
    $('#cli').focus()
    .elastic()
    .data('currentContext', currentContext)
    .data('disablePost', false);

    $('#cli').focus();
    SendCommand('Initialize');
});

function DisplayText(displayArray, element, scrollToBottom) {
    var uniqueId = $(element).find('.displayDiv').length;
    $(element).append("<div class='displayDiv' id='div" + uniqueId + "'></div>");
    element = '#div' + uniqueId;
    $(element).data('uniqueId', uniqueId);
    $(element).data('index', 0);
    $(element).data('displayArray', displayArray);
    $(element).data('scrollToBottom', scrollToBottom);
    DisplayTextExecute(element);
}

function DisplayTextExecute(element) {
    var uniqueId = $(element).data('uniqueId');
    var index = $(element).data('index');
    var displayArray = $(element).data('displayArray');
    var displayObject = displayArray[index];
    var caret = '#caret' + uniqueId;
    $(caret).data('Visible', true);

    if (index >= displayArray.length) {
        $(caret).remove();
        index = 0;
        $(element).data('index', index);
        letterIndex = 0;
        $(element).data('letterIndex', letterIndex);
        $('#cli').data('disablePost', false);
    }
    else {
        $(element).data('letterIndex', 0);
        var typeDisplay = '#typeDisplay' + uniqueId;
        var typeDisplayHtml = "<span id=\"typeDisplay" + uniqueId + "\" style=\"" + displayObject.Style + "\"></span>";
        var caretHtml = "<span id=\"caret" + uniqueId + "\">" + displayObject.CaretChar + "</span>";
        $(element).append(displayObject.InsertBefore + typeDisplayHtml + caretHtml + displayObject.InsertAfter);
        var blinkid = setInterval(function () {
            if ($(caret).data('Visible')) {
                $(caret).data('Visible', false);
                $(caret).html('&nbsp;');
            }
            else {
                $(caret).data('Visible', true);
                $(caret).html(displayObject.CaretChar);
            }
        }, 300);
        setTimeout(function () {
            if ((displayObject.PlaySound)&&(displayObject.Type)) {
                soundManager.play('beep');
            }
            clearInterval(blinkid);
            $(caret).show();
            $(element).data('intervalId',
            setInterval(function () {
                writeOneLetter(element);
            },
            displayObject.Speed));
        },
        displayObject.DelayBefore);
    }
}

function writeOneLetter(element) {
    var index = $(element).data('index');
    var letterIndex = $(element).data('letterIndex');
    var displayObject = $(element).data('displayArray')[index];
    var uniqueId = $(element).data('uniqueId');
    var typeDisplay = '#typeDisplay' + uniqueId;
    var caret = '#caret' + uniqueId;
    var scrollToBottom = $(element).data('scrollToBottom');
    $(caret).html(displayObject.CaretChar);
    if (displayObject.Type) {
        var char = displayObject.Text[letterIndex];
        $(typeDisplay).text($(typeDisplay).text() + char);
        letterIndex++;
        $(element).data('letterIndex', letterIndex);
    }
    else {
        $(typeDisplay).append(displayObject.Text);
        letterIndex = displayObject.Text.length;
        $(element).data('letterIndex', letterIndex);
    }
    if (scrollToBottom)
        $(window).scrollTo($('#bottom'), 0, { axis: 'y' });
    if (letterIndex >= displayObject.Text.length) {
        if ((displayObject.PlaySound)&&(displayObject.Type)) {
            soundManager.stop('beep');
        }
        index++;
        $(element).data('index', index);
        clearInterval($(element).data('intervalId'));
        var blinkid = setInterval(function () {
            if ($(caret).data('Visible')) {
                $(caret).data('Visible', false);
                $(caret).html('&nbsp;');
            }
            else {
                $(caret).data('Visible', true);
                $(caret).html(displayObject.CaretChar);
            }
        }, 300);
        setTimeout(function () {
            clearInterval(blinkid);
            $(typeDisplay).removeAttr('id');
            $(caret).remove();
            DisplayTextExecute(element);
        }, displayObject.DelayAfter);
    }
}

function ResizeCLI() {
    $('#cli').width($(window).width() * .75);
};

function DisplayLoading() {
    $('#loading').show()
    .data('step', 0)
    .data('intervalId',
        setInterval(function () {
            var loadingText = "Loading";
            var step = $('#loading').data('step');
            for (count = 0; count < step; count++) {
                loadingText += ".";
            }
            $('#loading').html(loadingText);
            step++;
            if (step > 50)
                step = 0;
            $('#loading').data('step', step);
        }, 500));
}

function HideLoading() {
    clearInterval($('#loading').data('intervalId'));
    $('#loading').html('');
    $('#loading').hide();
}

function SendCommand(cmd) {
    var commandString = cmd;
    if (commandString == null)
        commandString = $('#cli').val();
    if (!$('#cli').data('disablePost') && commandString != "") {
        var currentContext = $('#cli').data('currentContext');
        currentContext.CommandString = commandString;
        $('#cli').val('');
        $('#cli').data('disablePost', true);
        DisplayLoading();
        $.ajax({
            url: rootPath + currentContext.CurrentController + "/ExecuteCommand",
            data: JSON.stringify(currentContext),
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-i",
            success: function (response) { ReceiveResponse(response) },
            error: function (xhr, textStatus, errorThrown) {
                HideLoading();
                var displayArray = new Array();
                displayArray[0] = { Type: false, Text: "Error: ", DelayAfter: 1000, Speed: 10, CaretChar: "\u2588", InsertAfter: "", Style: "", DelayBefore: 0, InsertBefore: "" };
                displayArray[1] = { Type: true, Text: xhr.statusText, Speed: 10, InsertAfter: "<br />", CaretChar: "\u2588", InsertAfter: "<br />", Style: "", DelayBefore: 0, DelayAfter: 0, InsertBefore: "" };
                displayArray[2] = { Type: false, Text: "Command String: ", DelayAfter: 1000, Speed: 10, CaretChar: "\u2588", InsertAfter: "", Style: "", DelayBefore: 0, InsertBefore: "" };
                displayArray[3] = { Type: true, Text: commandString, Speed: 10, CaretChar: "\u2588", InsertAfter: "<br />", Style: "", DelayBefore: 0, DelayAfter: 0, InsertBefore: "" };
                DisplayText(displayArray, '#terminal');
            }
        });
    }
}

function ReceiveResponse(response) {
    HideLoading();
    $('#cli').data('currentContext', response.CurrentContext);
    $('#context').html('');
    if (response.CurrentContext.ContextDisplay != null) {
        $('#context').html(response.CurrentContext.ContextDisplay);
        //$('#context').append('&nbsp;');
    }
    if (response.ClearScreen)
        $('#terminal').html('');
    if (response.DisplayArray != null) {
        if (response.DisplayArray.length > 0)
            DisplayText(response.DisplayArray, '#terminal', response.ScrollToBottom);
        else
            $('#cli').data('disablePost', false);
    }
    else
        $('#cli').data('disablePost', false);
    if (response.IsPassword && ($('textarea#cli').length > 0)) {
        var data1 = $('#cli').data('disablePost');
        var data2 = $('#cli').data('currentContext');
        $('#cli').replaceWith('<input type="password" id="cli" />');
        ResizeCLI();
        $('#cli').data('disablePost', data1).data('currentContext', data2);
        setTimeout(function () { $('#cli').focus(); }, 1);
    }
    else if (!response.IsPassword && ($('input#cli').length > 0)) {
        var data1 = $('#cli').data('disablePost');
        var data2 = $('#cli').data('currentContext');
        $('#cli').replaceWith('<textarea id="cli" rows="1"></textarea>');
        ResizeCLI();
        $('#cli').data('disablePost', data1).data('currentContext', data2);
        setTimeout(function () { $('#cli').elastic().focus(); }, 1);
    }
    if (response.EditText != null) {
        $('#cli').val(response.EditText);
        ResizeCLI();
        $('#cli').elastic().focus();
    }
}

// Events
soundManager.onload = function () {
    soundManager.createSound(
        {
            id: 'beep',
            url: rootPath + 'Content/sounds/digitalbeep2.mp3',
            autoLoad: true,
            stream: true,
            multishot: true,
            loops: 999999999,
            volume: 50
        });
    soundManager.load('beep');
}

$(window).resize(function () {
    ResizeCLI();
});

$('.transmit').live('click', function (e) {
    $('#cli').focus();
    $('#cli').val($('#cli').val() + $(this).text());
});

$('*').live('keydown', function (e) {
    var key = e.keyCode;
    if (e.shiftKey || e.altKey || e.ctrlKey) {
        $('#cli').data('continueKeyPress', false);
    }
    else {
        $('#cli').data('continueKeyPress', true);
    }

    if (e.shiftKey && key == 13) {
        if ($('#cli').data('hasFocus') == true) {
            $('#cli').insertAtCaret('\n');
            $(window).scrollTo($('#bottom'), 0, { axis: 'y' });
            return false;
        }
    }
});

$('*').live('keypress', function (e) {
    if ($('#cli').data('continueKeyPress')) {
        var key = e.keyCode ? e.keyCode : e.charCode;
        if (e.keyCode && e.charCode)
            key = e.charCode;
        if (key != 13) {
            var letter = String.fromCharCode(key);
            $('#text').append(key + "|" + letter + "|" + $('#cli').data('hasFocus') + " "); //debug line
            if ($('#cli').data('hasFocus') == false) {
                $(window).scrollTo($('#bottom'), 0, { axis: 'y' });
                $('#cli').focus();
                $('#cli').val($('#cli').val() + letter);
                return false;
            }
        }
        else {
            SendCommand(null);
            return false;
        }
    }
});

$('#cli').live('focus', function () {
    $('#cli').data('hasFocus', true);
});

$('#cli').live('blur', function () {
    $('#cli').data('hasFocus', false);
});

// JQuery extensions

jQuery.fn.extend({
    insertAtCaret: function (myValue)
    {
        return this.each(function (i)
        {
            if (document.selection)
            {
                this.focus();
                sel = document.selection.createRange();
                sel.text = myValue;
                this.focus();
            }
            else if (this.selectionStart || this.selectionStart == '0')
            {
                var startPos = this.selectionStart;
                var endPos = this.selectionEnd;
                var scrollTop = this.scrollTop;
                this.value = this.value.substring(0, startPos) + myValue + this.value.substring(endPos, this.value.length);
                this.focus();
                this.selectionStart = startPos + myValue.length;
                this.selectionEnd = startPos + myValue.length;
                this.scrollTop = scrollTop;
            } else
            {
                this.value += myValue;
                this.focus();
            }
        })
    }
});