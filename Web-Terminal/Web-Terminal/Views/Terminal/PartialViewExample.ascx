<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<string>>" %>
<div style="border: dashed 2px #00ff00; padding: 10px;">
    This is an example of a partial view, rendered to a string, added to the Text property
    of a DisplayObject, added to the DisplayArray of the ResultObject, and passed back
    to the client for awesomeness.
    <br />
    <br />
    My model in this view is IEnumerable&lt;string&gt; and I passed in an example string
    list that I will now read from the view. Here it goes:
    <br />
    <br />
    <table border="1" cellpadding="10" style="border-color: #00ff00;">
        <% foreach (string s in Model)
           { %>
        <tr>
            <td>
                <%: s %>
            </td>
        </tr>
        <% } %>
    </table>
    <br />
    <br />
    Because it's a partial view, doing complex HTML is easy. Putting those strings from
    the model into a HTML table was not a problem at all.
    <br />
    <br />
    Now here is an example of some custom client script within a partial view:
    <br />
    <div id="clickme" style="width:100px; color: Black; background-color: #00ff00; text-align: center; cursor: crosshair;">
        Click Me!
    </div>
    <script type="text/javascript">
        $('#clickme').bind('click', function () {
            alert("Ouch, you didn't have to click so hard.");
        });
    </script>
</div>
