//===============================================================================================================
// System  : Sandcastle Help File Builder
// File    : TOC.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/26/2013
// Note    : Copyright 2006-2013, Eric Woodruff, All rights reserved
// Compiler: JavaScript
//
// This file contains the methods necessary to implement a simple tree view for the table of content with a
// resizable splitter and Ajax support to load tree nodes on demand.  It also contains the script necessary to do
// full-text searches.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code.  It can also be found at the project website: http://SHFB.CodePlex.com.  This
// notice, the author's name, and all copyright notices must remain intact in all applications, documentation,
// and source files.
//
// Version     Date     Who  Comments
// ==============================================================================================================
// 1.3.0.0  09/12/2006  EFW  Created the code
// 1.4.0.2  06/15/2007  EFW  Reworked to get rid of frame set and to add support for Ajax to load tree nodes on
//                           demand.
// 1.5.0.0  06/24/2007  EFW  Added full-text search capabilities
// 1.6.0.7  04/01/2008  EFW  Merged changes from Ferdinand Prantl to add a website keyword index.  Added support
//                           for "topic" query string option.
// 1.9.4.0  02/21/2012  EFW  Merged code from Thomas Levesque to show direct link and support other page types
//                           like PHP.
// 1.9.5.0  07/25/2012  EFW  Made changes to support IE 10.
// 1.9.8.0  07/26/2013  EFW  Merged changes from Dave Dansey to sync to toc when the topic URL parameter is used
//===============================================================================================================

// IE and Chrome flags
var isIE = (navigator.userAgent.indexOf("MSIE") >= 0);
var isIE10OrLater = /MSIE 1\d\./.test(navigator.userAgent);
var isChrome = (navigator.userAgent.indexOf("Chrome") >= 0);

// Page extension
var pageExtension = ".aspx";

// Minimum width of the TOC div
var minWidth = 100;

// Elements and sizing info
var divTOC, divSizer, topicContent, divNavOpts, divTree, docBody,
    maxWidth, offset, txtSearchText, chkSortByTitle;

// Last node selected
var lastNode, lastSearchNode, lastIndexNode;

// Last page with keyword index
var currentIndexPage = 0;

// XML Doc of the TOC
var xmlTOCDoc

//============================================================================

// Initialize the tree view and resize the content.  Pass it the page extension to use (i.e. ".aspx")
// for loading TOC element, index keywords, searching, etc.
function Initialize(extension)
{
    docBody = document.getElementsByTagName("body")[0];
    divTOC = document.getElementById("TOCDiv");
    divSizer = document.getElementById("TOCSizer");
    topicContent = document.getElementById("TopicContent");
    divNavOpts = document.getElementById("divNavOpts");
    divTree = document.getElementById("divTree");
    txtSearchText = document.getElementById("txtSearchText");
    chkSortByTitle = document.getElementById("chkSortByTitle");

    // Set the page extension if specified
    if(typeof(extension) != "undefined" && extension != "")
        pageExtension = extension;

    // The sizes are bit off in FireFox
    ResizeTree();
    SyncTOC();

    topicContent.onload = SyncTOC;

    // Use an alternate default page if a topic is specified in the query string
    var queryString = document.location.search;

    if(queryString != "")
    {
        var idx, options = queryString.split(/[\?\=\&]/);

        for(idx = 0; idx < options.length; idx++)
            if(options[idx] == "topic" && idx + 1 < options.length)
            {
                // Don't allow references outside the current site
                if(options[idx + 1].length > 1 && options[idx + 1][0] != '/' && options[idx + 1][0] != '.')
                    topicContent.src = options[idx + 1];

                break;
            }
    }
}

//============================================================================
// Navigation and expand/collaps code

// Synchronize the table of content with the selected page if possible
function SyncTOC()
{
    var idx, anchor, base, href, url, anchors, treeNode, saveNode;

    base = window.location.href;
    base = base.substr(0, base.lastIndexOf("/") + 1);

    if(base.substr(0, 5) == "file:" && base.substr(0, 8) != "file:///")
        base = base.replace("file://", "file:///");

    url = GetCurrentUrl();

    if(url == "")
        return false;

    if(url.substr(0, 5) == "file:" && url.substr(0, 8) != "file:///")
        url = url.replace("file://", "file:///");

    while(true)
    {
        anchors = divTree.getElementsByTagName("A");
        anchor = null;

        for(idx = 0; idx < anchors.length; idx++)
        {
            href = anchors[idx].href;

            if(href.substring(0, 7) != 'http://' && href.substring(0, 8) != 'https://' &&
              href.substring(0, 7) != 'file://')
                href = base + href;

            if(href == url)
            {
                anchor = anchors[idx];
                break;
            }
        }

        if(anchor == null)
        {
            // If it contains a "#", strip anything after that and try again
            if(url.indexOf("#") != -1)
            {
                url = url.substr(0, url.indexOf("#"));
                continue;
            }

            LoadTOC(url);
            return;
        }

        break;
    }

    // If found, select it and find the parent tree node
    SelectNode(anchor);
    saveNode = anchor;
    lastNode = null;

    while(anchor != null)
    {
        if(anchor.className == "TreeNode")
        {
            treeNode = anchor;
            break;
        }

        anchor = anchor.parentNode;
    }

    // Expand it and all of its parents
    while(anchor != null)
    {
        Expand(anchor);

        anchor = anchor.parentNode;

        while(anchor != null)
        {
            if(anchor.className == "TreeNode")
                break;

            anchor = anchor.parentNode;
        }
    }

    lastNode = saveNode;

    // Scroll the node into view
    var windowTop = lastNode.offsetTop - divTree.offsetTop - divTree.scrollTop;
    var windowBottom = divTree.clientHeight - windowTop - lastNode.offsetHeight;

    if(windowTop < 0)
        divTree.scrollTop += windowTop - 30;
    else
        if(windowBottom < 0)
            divTree.scrollTop -= windowBottom - 30;
}

// Search an array to see if it contains the given object
function contains(a, obj)
{
    for(var i = 0; i < a.length; i++)
        if(a[i] === obj)
            return true;

    return false;
}

// Load the given XML document
function loadXMLDoc(dname)
{
    try
    {
        // Internet Explorer
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
    }
    catch(e)
    {
        try
        {
            // Firefox, Mozilla, Opera, etc. (Doesn't currently work in Chrome)
            xmlDoc = document.implementation.createDocument("","",null);
        }
        catch(e)
        {
//            alert(e.message);
        }
    }

    try
    {
        xmlDoc.async = false;
        xmlDoc.load(dname);
        return(xmlDoc);
    }
    catch(e)
    {
//        alert(e.message);
    }

    return null;
}

// Get the parent TOC IDs from the TOC XML file
function GetParentTOCIds(target)
{
    if(xmlTOCDoc == null)
    {
        // Load the TOC XML
        xmlTOCDoc=loadXMLDoc("WebTOC.xml");

        if(xmlTOCDoc == null)
            return new Array();
    }

    // Get all TOC nodes
    x = xmlTOCDoc.getElementsByTagName("HelpTOCNode");

    // Iterate nodes looking for the target
    var targetNode = null;

    for(i = 0; i < x.length; i++)
    {
        var id = x[i].getAttribute('Url');

        id = id.substring(id.lastIndexOf("/") + 1, id.length - (id.length - id.lastIndexOf(".")));

        if(id == target)
        {
            targetNode = x[i];
            break;
        }
    }

    // Build an array of parent ids of the target node
    var ids = new Array();

    if(targetNode != null)
    {
        var index = 0;

        while(targetNode.parentNode.tagName == "HelpTOCNode")
        {
            targetNode = targetNode.parentNode;
            ids[index] = targetNode.getAttribute('Id');
            index = index + 1;
        }
    }

    return ids
}

// Load the TOC and expand all parent nodes down to the given entry
function LoadTOC(url)
{
    // Extract the target id from the url
    var target = url.substring(url.lastIndexOf("/") + 1, url.length - (url.length - url.lastIndexOf(".")));

    // Get an array of parent id's
    var idList = GetParentTOCIds(target);

    var divIdx, childIdx, img, divs = document.getElementsByTagName("DIV");
    var childNodes, child, div;

    // Loop through all DIV tags, looking for the next one to lazy-load
    for(divIdx = 0; divIdx < divs.length; divIdx++)
        if(divs[divIdx].className == "Hidden" || divs[divIdx].className == "Visible")
        {
            childNodes = divs[divIdx].parentNode.childNodes;

            for(childIdx = 0; childIdx < childNodes.length; childIdx++)
            {
                child = childNodes[childIdx];

                if(child.className == "TreeNodeImg")
                    img = child;

                if(child.className == "Hidden" || child.className == "Visible")
                {
                    div = child;
                    break;
                }
            }

            if(div.className == "Hidden" && contains(idList,div.id))
            {
                div.className = "Visible";
                img.src = "interface-resources/Expanded.gif";

                if(div.innerHTML == "")
                    FillNodeAndTrySyncTOC(div)
            }
        }
}

// Lazy load the child TOC nodes and re-try to SyncTOC afterwards (if the TOC still can't be sync'd the proces
// will run again to expand the next parent down).
function FillNodeAndTrySyncTOC(div)
{
    var xmlHttp = GetXmlHttpRequest(), now = new Date();

    if(xmlHttp == null)
    {
        div.innerHTML = "<b>XML HTTP request not supported!</b>";
        return;
    }

    div.innerHTML = "Loading...";

    // Add a unique hash to ensure it doesn't use cached results
    xmlHttp.open("GET", "FillNode" + pageExtension + "?Id=" + div.id + "&hash=" + now.getTime(), true);

    xmlHttp.onreadystatechange = function()
    {
        if(xmlHttp.readyState == 4)
        {
            div.innerHTML = xmlHttp.responseText;

            SyncTOC();
        }
    }

    xmlHttp.send(null)
}

// Get the currently loaded URL from the IFRAME
function GetCurrentUrl()
{
    var base, url = "";

    try
    {
        url = window.frames["TopicContent"].document.URL.replace(/\\/g, "/");
    }
    catch(e)
    {
        // If this happens the user probably navigated to another frameset that didn't make itself the topmost
        // frameset and we don't have control of the other frame anymore.  In that case, just reload our index
        // page.
        base = window.location.href;
        base = base.substr(0, base.lastIndexOf("/") + 1);

        // Chrome is too secure and won't let you access frame URLs when running from the file system unless
        // you run Chrome with the "--disable-web-security" command line option.
        if(isChrome && base.substr(0, 5) == "file:")
        {
            alert("Chrome security prevents access to file-based frame URLs.  As such, the TOC will not work " +
                "with index.html.  Either run this website on a web server, run Chrome with the " +
                "'--disable-web-security' command line option, or use FireFox or Internet Explorer.");

            return "";
        }

        if(base.substr(0, 5) == "file:" && base.substr(0, 8) != "file:///")
            base = base.replace("file://", "file:///");

        // Use lowercase on name for case-sensitive servers
        if(base.substr(0, 5) == "file:")
            top.location.href = base + "index.html";
        else
            top.location.href = base + "index" + pageExtension;
    }

    return url;
}

// Expand or collapse all nodes
function ExpandOrCollapseAll(expandNodes)
{
    var divIdx, childIdx, img, divs = document.getElementsByTagName("DIV");
    var childNodes, child, div, link, img;

    for(divIdx = 0; divIdx < divs.length; divIdx++)
        if(divs[divIdx].className == "Hidden" || divs[divIdx].className == "Visible")
        {
            childNodes = divs[divIdx].parentNode.childNodes;

            for(childIdx = 0; childIdx < childNodes.length; childIdx++)
            {
                child = childNodes[childIdx];

                if(child.className == "TreeNodeImg")
                    img = child;

                if(child.className == "Hidden" || child.className == "Visible")
                {
                    div = child;
                    break;
                }
            }

            if(div.className == "Visible" && !expandNodes)
            {
                div.className = "Hidden";
                img.src = "interface-resources/Collapsed.gif";
            }
            else
                if(div.className == "Hidden" && expandNodes)
                {
                    div.className = "Visible";
                    img.src = "interface-resources/Expanded.gif";

                    if(div.innerHTML == "")
                        FillNode(div, true)
                }
        }
}

// Toggle the state of the specified node
function Toggle(node)
{
    var i, childNodes, child, div, link;

    childNodes = node.parentNode.childNodes;

    for(i = 0; i < childNodes.length; i++)
    {
        child = childNodes[i];

        if(child.className == "Hidden" || child.className == "Visible")
        {
            div = child;
            break;
        }
    }

    if(div.className == "Visible")
    {
        div.className = "Hidden";
        node.src = "interface-resources/Collapsed.gif";
    }
    else
    {
        div.className = "Visible";
        node.src = "interface-resources/Expanded.gif";

        if(div.innerHTML == "")
            FillNode(div, false)
    }
}

// Expand the selected node
function Expand(node)
{
    var i, childNodes, child, div, img;

    // If not valid, don't bother
    if(GetCurrentUrl() == "")
        return false;

    if(node.tagName == "A")
        childNodes = node.parentNode.childNodes;
    else
        childNodes = node.childNodes;

    for(i = 0; i < childNodes.length; i++)
    {
        child = childNodes[i];

        if(child.className == "TreeNodeImg")
            img = child;

        if(child.className == "Hidden" || child.className == "Visible")
        {
            div = child;
            break;
        }
    }

    if(lastNode != null)
        lastNode.className = "UnselectedNode";

    div.className = "Visible";
    img.src = "interface-resources/Expanded.gif";

    if(node.tagName == "A")
    {
        node.className = "SelectedNode";
        lastNode = node;
    }

    if(div.innerHTML == "")
        FillNode(div, false)

    return true;
}

// Set the style of the specified node to "selected"
function SelectNode(node)
{
    // If not valid, don't bother
    if(GetCurrentUrl() == "")
        return false;

    if(lastNode != null)
        lastNode.className = "UnselectedNode";

    node.className = "SelectedNode";
    lastNode = node;

    return true;
}

//============================================================================
// Ajax-related code used to fill the tree nodes on demand

function GetXmlHttpRequest()
{
    var xmlHttp = null;

    // If IE7, Mozilla, Safari, etc., use the native object.  Otherwise, use the ActiveX control for IE5.x and IE6.
    if(window.XMLHttpRequest)
        xmlHttp = new XMLHttpRequest();
    else
        if(window.ActiveXObject)
            xmlHttp = new ActiveXObject("MSXML2.XMLHTTP.3.0");

    return xmlHttp;
}

// Perform an AJAX-style request for the contents of a node and put the contents into the empty div
function FillNode(div, expandChildren)
{
    var xmlHttp = GetXmlHttpRequest(), now = new Date();

    if(xmlHttp == null)
    {
        div.innerHTML = "<b>XML HTTP request not supported!</b>";
        return;
    }

    div.innerHTML = "Loading...";

    // Add a unique hash to ensure it doesn't use cached results
    xmlHttp.open("GET", "FillNode" + pageExtension + "?Id=" + div.id + "&hash=" + now.getTime(), true);

    xmlHttp.onreadystatechange = function()
    {
        if(xmlHttp.readyState == 4)
        {
            div.innerHTML = xmlHttp.responseText;

            if(expandChildren)
                ExpandOrCollapseAll(true);
        }
    }

    xmlHttp.send(null)
}

//============================================================================
// Resizing code

// Resize the tree div so that it fills the document body
function ResizeTree()
{
    var y, newHeight;

    if(self.innerHeight)    // All but IE
        y = self.innerHeight;
    else    // IE - Strict
        if(document.documentElement && document.documentElement.clientHeight)
            y = document.documentElement.clientHeight;
        else    // Everything else
            if(document.body)
                y = document.body.clientHeight;

    newHeight = y - parseInt(divNavOpts.style.height, 10) - 6;

    if(newHeight < 50)
        newHeight = 50;

    divTree.style.height = newHeight;

    // Resize the content div
    ResizeContent();
}

// Resize the content div
function ResizeContent()
{
    // IE 10 sizes the frame like FireFox and Chrome
    if(isIE && !isIE10OrLater)
        maxWidth = docBody.clientWidth - 1;
    else
        maxWidth = docBody.clientWidth - 4;

    topicContent.style.width = maxWidth - (divSizer.offsetLeft + divSizer.offsetWidth);
    maxWidth -= minWidth;
}

// This is called to prepare for dragging the sizer div
function OnMouseDown(event)
{
    var x;

    // Make sure the splitter is at the top of the z-index
    divSizer.style.zIndex = 5000;

    // The content is in an IFRAME which steals mouse events so hide it while resizing
    topicContent.style.display = "none";

    if(isIE)
        x = window.event.clientX + document.documentElement.scrollLeft + document.body.scrollLeft;
    else
        x = event.clientX + window.scrollX;

    // Save starting offset
    offset = parseInt(divSizer.style.left, 10);

    if(isNaN(offset))
        offset = 0;

    offset -= x;

    if(isIE)
    {
        document.attachEvent("onmousemove", OnMouseMove);
        document.attachEvent("onmouseup", OnMouseUp);
        window.event.cancelBubble = true;
        window.event.returnValue = false;
    }
    else
    {
        document.addEventListener("mousemove", OnMouseMove, true);
        document.addEventListener("mouseup", OnMouseUp, true);
        event.preventDefault();
    }
}

// Resize the TOC and content divs as the sizer is dragged
function OnMouseMove(event)
{
    var x, pos;

    // Get cursor position with respect to the page
    if(isIE)
        x = window.event.clientX + document.documentElement.scrollLeft + document.body.scrollLeft;
    else
        x = event.clientX + window.scrollX;

    left = offset + x;

    // Adjusts the width of the TOC divs
    pos = (event.clientX > maxWidth) ? maxWidth : (event.clientX < minWidth) ? minWidth : event.clientX;

    divTOC.style.width = divTree.style.width = pos;

    if(!isIE)
        pos -= 8;

    divNavOpts.style.width = pos;

    // Resize the content div to fit in the remaining space
    ResizeContent();
}

// Finish the drag operation when the mouse button is released
function OnMouseUp(event)
{
    if(isIE)
    {
        document.detachEvent("onmousemove", OnMouseMove);
        document.detachEvent("onmouseup", OnMouseUp);
    }
    else
    {
        document.removeEventListener("mousemove", OnMouseMove, true);
        document.removeEventListener("mouseup", OnMouseUp, true);
    }

    // Show the content div again
    topicContent.style.display = "inline";
}
