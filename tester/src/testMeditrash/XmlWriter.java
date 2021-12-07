package testMeditrash;


import org.w3c.dom.Document;
import org.w3c.dom.Element;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import java.io.StringWriter;
import java.nio.charset.Charset;

public class XmlWriter {
    public static String createDoc(XElement rootElm, Charset charset) throws ParserConfigurationException, TransformerException {
        DocumentBuilderFactory df = DocumentBuilderFactory.newInstance();
        DocumentBuilder db;
        db = df.newDocumentBuilder();
        Document doc = db.newDocument();
        Element root = doc.createElement(rootElm.name);
        for (MutPair<String,String> atrPair: rootElm.attributes) {
            root.setAttribute(atrPair.getFirst(),atrPair.getSecond());
        }
        root.setTextContent(rootElm.value);
        doc.appendChild(root);
        setElement(doc,root,rootElm);


        TransformerFactory transformerFactory = TransformerFactory.newInstance();
        Transformer transformer = transformerFactory.newTransformer();
        transformer.setOutputProperty(OutputKeys.METHOD, "xml");
        transformer.setOutputProperty(OutputKeys.ENCODING, charset.name());
        transformer.setOutputProperty(OutputKeys.STANDALONE, "yes");
        transformer.setOutputProperty(OutputKeys.INDENT,"yes");
        transformer.setOutputProperty(OutputKeys.VERSION, "1.0");
        StringWriter writer = new StringWriter();
        transformer.transform(new DOMSource(doc), new StreamResult(writer));
        String output = writer.getBuffer().toString();
        return output;
    }

    public static String createDoc(XElement rootElm) throws ParserConfigurationException, TransformerException {
        DocumentBuilderFactory df = DocumentBuilderFactory.newInstance();
        DocumentBuilder db;
        db = df.newDocumentBuilder();
        Document doc = db.newDocument();
        Element root = doc.createElement(rootElm.name);
        for (MutPair<String,String> atrPair: rootElm.attributes) {
            root.setAttribute(atrPair.getFirst(),atrPair.getSecond());
        }
        root.setTextContent(rootElm.value);
        doc.appendChild(root);
        setElement(doc,root,rootElm);


        TransformerFactory transformerFactory = TransformerFactory.newInstance();
        Transformer transformer = transformerFactory.newTransformer();
        transformer.setOutputProperty(OutputKeys.METHOD, "xml");
        transformer.setOutputProperty(OutputKeys.ENCODING, "UTF-8");
        transformer.setOutputProperty(OutputKeys.STANDALONE, "yes");
        transformer.setOutputProperty(OutputKeys.INDENT,"yes");
        transformer.setOutputProperty(OutputKeys.VERSION, "1.0");
        StringWriter writer = new StringWriter();
        transformer.transform(new DOMSource(doc), new StreamResult(writer));
        String output = writer.getBuffer().toString();
        return output;
    }
    public static void setElement(Document doc, Element parentElement, XElement parent){
        for (XElement clidren: parent.children) {
            Element childrenElement = doc.createElement(clidren.name);
            for (MutPair<String,String> atrPair: clidren.attributes) {
                childrenElement.setAttribute(atrPair.getFirst(),atrPair.getSecond());
            }
            childrenElement.setTextContent(clidren.value);
            parentElement.appendChild(childrenElement);
            setElement(doc,childrenElement,clidren);
        }
    }
}
