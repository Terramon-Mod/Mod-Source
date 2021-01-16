// MoveDB Generator
// Outputs mvdb.json in run folder

var getJSON = require('get-json');
var Pokedex = require('pokedex'),
    pokedex = new Pokedex();

var db = [ ]

var id = 1 // Bulbasaur's Pokedex ID

createDBEntry(id) // Create DB entry for the specified mon

function createDBEntry(id) {
  var pokemon = {
    name: "",
    baseExp: 0,
    baseHp: 0,
    baseAtk: 0,
    baseDef: 0,
    baseSpAtk: 0,
    baseSpDef: 0,
    baseSpeed: 0,
    evYieldHp: 0,
    evYieldAtk: 0,
    evYieldDef: 0,
    evYieldSpAtk: 0,
    evYieldSpDef: 0,
    evYieldSpeed: 0,
    learnAtLevel: []
  }
  pokemon.name = capitalizeFirstLetter(pokedex.pokemon(id).name) // Get Pokemon name

  getJSON('https://pokeapi.co/api/v2/pokemon/' + id.toString(), function(error, res) {
    pokemon.baseExp = res.base_experience;

    pokemon.baseHp = res.stats[0].base_stat;
    if (res.stats[0].effort != 0) {
        pokemon.evYieldHp = res.stats[0].effort;
    }

    pokemon.baseAtk = res.stats[1].base_stat;
    if (res.stats[1].effort != 0) {
        pokemon.evYieldAtk = res.stats[1].effort;
    }

    pokemon.baseDef = res.stats[2].base_stat;
    if (res.stats[2].effort != 0) {
        pokemon.evYieldDef = res.stats[2].effort;
    }

    pokemon.baseSpAtk = res.stats[3].base_stat;
    if (res.stats[3].effort != 0) {
        pokemon.evYieldSpAtk = res.stats[3].effort;
    }

    pokemon.baseSpDef = res.stats[4].base_stat;
    if (res.stats[4].effort != 0) {
        pokemon.evYieldSpDef = res.stats[4].effort;
    }

    pokemon.baseSpeed = res.stats[5].base_stat;
    if (res.stats[5].effort != 0) {
        pokemon.evYieldSpeed = res.stats[5].effort;
    }

    for (var i = 0; i < res.moves.length; i++) {
      for (var j = 0; j < res.moves[i].version_group_details.length; j++) {
        if (res.moves[i].version_group_details[j].version_group.name == "red-blue") { // Gen 1 moves only for now

          n = res.moves[i].move.name.split("-");
          for(var k = 0; k < n.length; k++) {
            n[k] = capitalizeFirstLetter(n[k])
          }

          n = n.join("");

          if (res.moves[i].version_group_details[j].level_learned_at != 0) {
            pokemon.learnAtLevel.push( { [n]: res.moves[i].version_group_details[j].level_learned_at } );
          }
        }
      }
    }
    db.push(pokemon)
    console.log("Pushed " + pokemon.name + " to move database")
    if (id == 151) {
      require("fs").writeFile('./mvdb.json', JSON.stringify(db), 'utf8', function (err) {
        if (err) return console.log(err);
        console.log("Exported mvdb.json (" + (id - 1).toString() + " entries)")
        return;
      });
    }
    id += 1;
    createDBEntry(id)
  });
}

function capitalizeFirstLetter(string) {
  return string.charAt(0).toUpperCase() + string.slice(1);
}
