const table = require("./table.json");

level = 0

genErraticTable()

// Erratic table
function genErraticTable() {
  level += 1;
  var t = table[level];
  console.log(`{ ${level}, ${t["FIELD13"].replace(/,/g, "")} },`)
  genErraticTable()
}
