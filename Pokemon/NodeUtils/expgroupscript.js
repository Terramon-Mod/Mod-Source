// Exp group adder utility script

const fs = require("fs");
var getJSON = require('get-json')
var Pokedex = require('pokedex'),
    pokedex = new Pokedex();

var target;
var exp_group;

var data;
var linecounter = 0;
var pokecounter = 0;

start()

function start() {
  pokecounter++; // Go up counting pokedex
  if (pokecounter == 29 || pokecounter == 32 || pokecounter == 122 || pokecounter == 144 || pokecounter == 145 || pokecounter == 146) pokecounter++; // skip problematic pokemon like nidoran, mr. mime, and unimplemented ones like the legendary birds
  target = pokedex.pokemon(pokecounter).name.toUpperCase();
  fs.readFile(`./Normal/${target}/${target}.cs`, 'utf8', function(err, d) {
      if (err) throw err;
      data = d.split("\r\n"); // Convert to array
      getJSON('https://pokeapi.co/api/v2/growth-rate/1', function(error, res) {
          for (i = 0; i < res.pokemon_species.length; i++) {
              if (res.pokemon_species[i].name == target.toLowerCase()) {
                  x = "-" + res.name;
                  exp_group = x.replace(/(^|\/|-)(\S)/g, s => s.toUpperCase()).replace(/-/g, "");
                  exp_group = exp_group.charAt(0).toUpperCase() + exp_group.slice(1)
              }
          }
          if (exp_group != undefined) {
              addToFile();
              return;
          }
          getJSON('https://pokeapi.co/api/v2/growth-rate/2', function(error, res) {
              for (i = 0; i < res.pokemon_species.length; i++) {
                  if (res.pokemon_species[i].name == target.toLowerCase()) {
                      x = "-" + res.name;
                      exp_group = x.replace(/(^|\/|-)(\S)/g, s => s.toUpperCase()).replace(/-/g, "");
                      exp_group = exp_group.charAt(0).toUpperCase() + exp_group.slice(1)
                  }
              }
              if (exp_group != undefined) {
                  addToFile();
                  return;
              }
              getJSON('https://pokeapi.co/api/v2/growth-rate/3', function(error, res) {
                  for (i = 0; i < res.pokemon_species.length; i++) {
                      if (res.pokemon_species[i].name == target.toLowerCase()) {
                          x = "-" + res.name;
                          exp_group = x.replace(/(^|\/|-)(\S)/g, s => s.toUpperCase()).replace(/-/g, "");
                          exp_group = exp_group.charAt(0).toUpperCase() + exp_group.slice(1)
                      }
                  }
                  if (exp_group != undefined) {
                      addToFile();
                      return;
                  }
                  getJSON('https://pokeapi.co/api/v2/growth-rate/4', function(error, res) {
                      for (i = 0; i < res.pokemon_species.length; i++) {
                          if (res.pokemon_species[i].name == target.toLowerCase()) {
                              x = "-" + res.name;
                              exp_group = x.replace(/(^|\/|-)(\S)/g, s => s.toUpperCase()).replace(/-/g, "");
                              exp_group = exp_group.charAt(0).toUpperCase() + exp_group.slice(1)
                          }
                      }
                      if (exp_group != undefined) {
                          addToFile();
                          return;
                      }
                      getJSON('https://pokeapi.co/api/v2/growth-rate/5', function(error, res) {
                          for (i = 0; i < res.pokemon_species.length; i++) {
                              if (res.pokemon_species[i].name == target.toLowerCase()) {
                                  x = "-" + res.name;
                                  exp_group = x.replace(/(^|\/|-)(\S)/g, s => s.toUpperCase()).replace(/-/g, "");
                                  exp_group = exp_group.charAt(0).toUpperCase() + exp_group.slice(1)
                              }
                          }
                          if (exp_group != undefined) {
                              addToFile();
                              return;
                          }
                          getJSON('https://pokeapi.co/api/v2/growth-rate/6', function(error, res) {
                              for (i = 0; i < res.pokemon_species.length; i++) {
                                  if (res.pokemon_species[i].name == target.toLowerCase()) {
                                      x = "-" + res.name;
                                      exp_group = x.replace(/(^|\/|-)(\S)/g, s => s.toUpperCase()).replace(/-/g, "");
                                      exp_group = exp_group.charAt(0).toUpperCase() + exp_group.slice(1)
                                  }
                              }
                              if (exp_group != undefined) {
                                  addToFile();
                                  return;
                              }
                          });
                      });
                  });
              });
          });
      });
  });
}

function addToFile() {
    if (exp_group == "Medium") exp_group = "MediumFast";
    data.forEach(function(line) {
        if (line == "using Terraria.ModLoader;") {
            data[linecounter] = line + "\r\nusing static Terramon.Pokemon.ExpGroups;"
        }
        if (line.includes("public override PokemonType[] PokemonTypes")) {
            data[linecounter] = line + `\r\n\r\n        public virtual ExpGroup ExpGroup => ExpGroup.${exp_group};`
        }
        linecounter++;
    })
    console.log("Completed for " + target)
    linecounter = 0;
    exp_group = (function () { return; })();
    fs.writeFile(`./Normal/${target}/${target}.cs`, data.join("\r\n"), 'utf8', function (err) {
	if (err) return console.log(err);
	start()
    });
}
