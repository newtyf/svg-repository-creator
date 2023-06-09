using AngleSharp.Dom;

namespace EmbedRepoGithub;

public class SvgRepo
{
    public static IElement Create(IDocument document, string repoTitle, string repoDescription, string repoLanguage,
        string languageColor, string borderColor = "#30363d")
    {
        IElement svgElement = document.CreateElement("svg");
        svgElement.SetAttribute("xmlns", "http://www.w3.org/2000/svg");
        svgElement.SetAttribute("width", "400");
        svgElement.SetAttribute("height", "102");
        svgElement.SetAttribute("style", "background-color: #0d1116");

        IElement[] Lines = CreateRectangle(document, borderColor);
        foreach (var line in Lines)
        {
            svgElement.Append(line);
        }

        IElement book = CreateBook(document);
        svgElement.Append(book);

        IElement title = SetTitle(document, repoTitle);
        svgElement.Append(title);

        IElement description = SetDescription(document, repoDescription);
        svgElement.Append(description);

        IElement language = SetLanguage(document, repoLanguage);
        svgElement.Append(language);

        IElement circle = CreateCircle(document, languageColor);
        svgElement.Append(circle);

        return svgElement;
    }

    private static IElement[] CreateRectangle(IDocument document, string borderColor)
    {
        IElement rectangleTop = document.CreateElement("rect");
        rectangleTop.SetAttribute("x", "0");
        rectangleTop.SetAttribute("y", "0");
        rectangleTop.SetAttribute("width", "400");
        rectangleTop.SetAttribute("height", "2");
        rectangleTop.SetAttribute("fill", borderColor);

        IElement rectangleRigth = document.CreateElement("rect");
        rectangleRigth.SetAttribute("x", "398");
        rectangleRigth.SetAttribute("y", "0");
        rectangleRigth.SetAttribute("width", "2");
        rectangleRigth.SetAttribute("height", "100");
        rectangleRigth.SetAttribute("fill", borderColor);

        IElement rectangleDown = document.CreateElement("rect");
        rectangleDown.SetAttribute("x", "0");
        rectangleDown.SetAttribute("y", "100");
        rectangleDown.SetAttribute("width", "400");
        rectangleDown.SetAttribute("height", "2");
        rectangleDown.SetAttribute("fill", borderColor);

        IElement rectangleLeft = document.CreateElement("rect");
        rectangleLeft.SetAttribute("x", "0");
        rectangleLeft.SetAttribute("y", "0");
        rectangleLeft.SetAttribute("width", "2");
        rectangleLeft.SetAttribute("height", "100");
        rectangleLeft.SetAttribute("fill", borderColor);

        IElement[] arr = new[] { rectangleTop, rectangleRigth, rectangleDown, rectangleLeft };
        return arr;
    }

    private static IElement CreateBook(IDocument document)
    {
        IElement book = document.CreateElement("svg");
        book.SetAttribute("xmlns", "http://www.w3.org/2000/svg");
        book.SetAttribute("x", "20");
        book.SetAttribute("y", "20");
        book.SetAttribute("width", "16");
        book.SetAttribute("height", "16");
        book.SetAttribute("fill", "gray");
        book.SetAttribute("viewBox", "0 0 16 16");

        IElement pathOne = document.CreateElement("path");
        pathOne.SetAttribute("fill-rule", "evenodd");
        pathOne.SetAttribute("d",
            "M6 8V1h1v6.117L8.743 6.07a.5.5 0 0 1 .514 0L11 7.117V1h1v7a.5.5 0 0 1-.757.429L9 7.083 6.757 8.43A.5.5 0 0 1 6 8z");

        IElement pathTwo = document.CreateElement("path");
        pathTwo.SetAttribute("d",
            "M3 0h10a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2v-1h1v1a1 1 0 0 0 1 1h10a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H3a1 1 0 0 0-1 1v1H1V2a2 2 0 0 1 2-2z");

        IElement pathThree = document.CreateElement("path");
        pathThree.SetAttribute("d",
            "M1 5v-.5a.5.5 0 0 1 1 0V5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1H1zm0 3v-.5a.5.5 0 0 1 1 0V8h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1H1zm0 3v-.5a.5.5 0 0 1 1 0v.5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1H1z");

        book.Append(pathOne);
        book.Append(pathTwo);
        book.Append(pathThree);
        return book;
    }

    private static IElement SetTitle(IDocument document, string repoTitle)
    {
        IElement titleElement = document.CreateElement("text");
        titleElement.SetAttribute("x", "43");
        titleElement.SetAttribute("y", "33");
        titleElement.TextContent = repoTitle;
        titleElement.SetAttribute("style", "font-size: 14px; font-weight: bold; font-family: Arial");
        titleElement.SetAttribute("fill", "#3081f7");

        return titleElement;
    }

    private static IElement SetDescription(IDocument document, string repoDescription)
    {
        IElement descriptionElement = document.CreateElement("text");
        descriptionElement.SetAttribute("x", "22");
        descriptionElement.SetAttribute("y", "58");
        descriptionElement.TextContent = repoDescription;
        descriptionElement.SetAttribute("style", "font-size: 12px; font-weight: bold; font-family: Arial");
        descriptionElement.SetAttribute("fill", "#67707a");

        return descriptionElement;
    }

    private static IElement SetLanguage(IDocument document, string repoLanguage)
    {
        IElement languageElement = document.CreateElement("text");
        languageElement.SetAttribute("x", "40");
        languageElement.SetAttribute("y", "83");
        languageElement.TextContent = repoLanguage;
        // font-weight: bold;
        languageElement.SetAttribute("style", "font-size: 12px; font-family: Arial");
        languageElement.SetAttribute("fill", "#67707a");

        return languageElement;
    }

    private static IElement CreateCircle(IDocument document, string languageColor)
    {
        IElement languageElement = document.CreateElement("circle");
        languageElement.SetAttribute("cx", "29");
        languageElement.SetAttribute("cy", "78");
        languageElement.SetAttribute("r", "6");
        // font-weight: bold;
        languageElement.SetAttribute("fill", languageColor);

        return languageElement;
    }
}