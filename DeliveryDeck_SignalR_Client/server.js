const http = require("http");
const fs = require("fs");

const host = "localhost";
const port = "8080";

const server = http.createServer((req, res) => {
    res.statusCode = 200;
    if (req.url === "/") {
        res.setHeader("Content-type", "text/html");
        html = fs.readFileSync('./index.html');
        res.end(html);
    }

    res.end("cum!!");
});

server.listen(port, host);