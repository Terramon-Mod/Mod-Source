// Base stat adder utility script

const fs = require("fs");
var getJSON = require('get-json')
var Pokedex = require('pokedex'),
    pokedex = new Pokedex();

var target;
var stats = { };

var data;
var linecounter = 0;
var pokecounter = 0;

start()

function start() {
  pokecounter++; // Go up counting pokedex
  if (pokecounter == 29 || pokecounter == 32 || pokecounter == 122 || pokecounter == 144 || pokecounter == 145 || pokecounter == 146) pokecounter++; // skip problematic pokemon like nidoran, mr. mime, and unimplemented ones like the legendary birds
  target = pokedex.pokemon(pokecounter).name.toUpperCase();
  fs.readFile(`./Normal/${target}/${target}.cs`, 'utf8', function(err, d) {
      data = d.split("\n");
      getJSON('https://pokeapi.co/api/v2/pokemon/' + pokecounter.toString(), function(error, res) {
        stats.hp = res.stats[0].base_stat.toString();
        stats.attack = res.stats[1].base_stat.toString();
        stats.defence = res.stats[2].base_stat.toString();
        stats.spatk = res.stats[3].base_stat.toString();
        stats.spdef = res.stats[4].base_stat.toString();
        stats.speed = res.stats[5].base_stat.toString();
        addToFile()
      });
  });
}

function addToFile() {
  console.log(data)
  data.forEach(function(line) {
      if (line.includes("ExpGroup ExpGroup")) {
          data[linecounter] = line + `public override int MaxHP => ${stats.hp}; public override int PhysicalDamage => ${stats.attack}; public override int PhysicalDefence => ${stats.defence}; public override int SpecialDamage => ${stats.spatk}; public override int SpecialDefence => ${stats.spdef}; public override int Speed => ${stats.speed};`
      }
      linecounter++;
  });
  linecounter = 0;
  stats = { }
  console.log("Completed BASE STATS for " + target)
  fs.writeFile(`./Normal/${target}/${target}.cs`, data.join("\r\n"), 'utf8', function (err) {
    if (err) return console.log(err);
    start()
  });
}

// public override int MaxHP => 45;
//public override int PhysicalDamage => 49;
//        public override int SpecialDamage => 65;
//        public override int SpecialDefence => 65;
  //      public override int Speed => 45;
