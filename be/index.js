const express = require("express");
const bodyParser = require("body-parser");
const cors = require("cors");
const { MongoClient } = require("mongodb");

const url = "mongodb://127.0.0.1:27017/myProject";
const client = new MongoClient(url);

const app = express();

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
app.use(cors());

// Database Name

client.connect(url, function (err, db) {
  if (err) throw err;
  console.log("Connected to database");
  // Perform CRUD operations here
  db.close();
});

app.get("/", (req, res) => {
  res.send({
    res: "Hello World!",
  });
});

app.get("/computer", async (req, res) => {
  const collection = client.db().collection("Computer");
  const data = await collection.find({}).toArray();
  return res.send({
    res: data,
  });
});

app.post("/infoComputer", async (req, res) => {
  const collection = client.db().collection("Computer");
  await collection.insertOne({ ...req.body });
  return res.send({
    res: "Hello World!",
  });
});

app.listen(3000, () => {
  console.log("Server is running on port 3000");
});
