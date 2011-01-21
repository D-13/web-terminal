<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Terminal</title>
    <%--Styles--%>
    <style type="text/css">
        @font-face
        {
            font-family: 'Pigpen';
            src: url('<%: Url.Content("~/Content/Fonts/BabelStonePigpen.ttf") %>');
        }
    </style>
    <!--[if IE]>
    <style type="text/css">
        @font-face
        {
            font-family: 'Pigpen';
            src: url('<%: Url.Content("~/Content/Fonts/BabelStonePigpen.eot") %>');
        }
    </style>
    <![endif]-->
    <link href="<%: Url.Content("~/Content/Styles/Terminal.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%: Url.Content("~/Content/Styles/jquery.jscrollpane.css") %>" rel="stylesheet"
        type="text/css" />
    <%--Scripts--%>
    <script type="text/javascript">
        var rootPath = '<%: Url.Content("~/") %>';
    </script>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/jquery-ui.min.js"></script>
    <script src="<%: Url.Content("~/Scripts/soundmanager2-nodebug-jsmin.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.jscrollpane.mousewheel.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.jscrollpane.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.scrollTo-min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/json2.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.elastic.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/TerminalFramework.js") %>" type="text/javascript"></script>
</head>
<body>
    <div id="terminal">
    </div>
    <div id="notify" style="display: none;">
    </div>
    <div id="loading" style="display: none;">
    </div>
    <table cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="vertical-align: top;">
                <div style="height: 3px;">
                </div>
                <span id="context"></span>&gt;&nbsp;
            </td>
            <td style="vertical-align: top;">
                <textarea id="cli" rows="1"></textarea>
            </td>
        </tr>
    </table>
    <div id="bottom">
    </div>
</body>
</html>
