const sceneInfos = [];
let currentScene;

function findSprite(sceneName, spriteName) {
    const sceneInfo = sceneInfos[sceneName];
    return sceneInfo.phaserScene.children.list.find(child => {
        return child.type === "Sprite" && child.name === spriteName
    });
}

function addHandler(sceneName, spriteName, handlerName) {
    const sprite = findSprite(sceneName, spriteName);

    sprite.setInteractive({ pixelPerfect: true });

    sprite.on('pointerup', (pointer, gameObject, event) => {
        const sceneInfo = sceneInfos[sceneName];
        sceneInfo.dotNetScene.invokeMethodAsync(handlerName, spriteName);
    });
}

function setSpriteCrop(sceneName, spriteName, x, y, width, height) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.setCrop(x, y, width, height);
}

function setSpriteScale(sceneName, spriteName, scale) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.scale = scale;
}

function getSpriteData(sceneName, spriteName, key) {
    const sprite = findSprite(sceneName, spriteName);
    return sprite.getData(key);
}

function setSpriteData(sceneName, spriteName, key, value) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.setData(key, value);
}

function addSprite(sceneName, spriteName, imageName, x, y, scale = null, interactive = false) {

    const sceneInfo = sceneInfos[sceneName];
    const sprite = sceneInfo.phaserScene.add.sprite(x, y, imageName);
    sprite.name = spriteName; // TODO Use same strategy for scenes.

    if (scale) {
        sprite.setScale(scale);
    }

}

function addRectangle(sceneName, x, y, width, height, color) {

    const sceneInfo = sceneInfos[sceneName];
    var graphics = sceneInfo.phaserScene.add.graphics({ fillStyle: { color: color } });

    var rect = new Phaser.Geom.Rectangle(x, y, width, height);

    graphics.fillRectShape(rect);
}

function removeSprite(sceneName, spriteName) {

    const scene = scenes[sceneName];
    const sprite = scene.children.list.find(child => { // TODO Extract to function
        return child.type === "Sprite" && child.name === spriteName
    });
    sprite.destroy();
}

function switchScene(from, to) {
    game.scene.stop(from);
    game.scene.start(to);
}

function startScene(scene) {
    game.scene.start(scene);
}

function stopScene(scene) {
    game.scene.stop(scene);
}

function registerScene(name, dotNetScene) {

//    console.log(name);
//    console.log(dotNetScene);

    var phaserScene = new Phaser.Scene(name);
    sceneInfos[name] = {
        phaserScene: phaserScene,
        dotNetScene: dotNetScene
    };

    // TODO Use pack instead of hard coded: https://phaser.io/examples/v3/view/scenes/swapping-scenes
    phaserScene.preload = function () {
        this.load.image('brush', './assets/brush.png');
        this.load.image('logo', './assets/logo.png');
        this.load.image('world', './assets/world.png');
        this.load.image('wally', './assets/wally.png');
        this.load.image('moodlevel', './assets/moodlevel.png');
    };

    phaserScene.create = function () {
        dotNetScene.invokeMethod('create');
    }
}

let game;

function startPhaser(container, title) {

    const config = {
        type: Phaser.AUTO,
        parent: container,
        width: 800,
        height: 600,
        scene: Object.keys(sceneInfos).map(key => sceneInfos[key].phaserScene),
        title: title
      };
      
      game = new Phaser.Game(config);
}




// this.horse = this.add.sprite(400, 400, 'horse');
// this.horse.setInteractive({ pixelPerfect: true });
// this.horse.inputEnabled = true;
// this.horse.input.pixelPerfectOver = true;
// // this.horse.input.useHandCursor = true;
// this.horse.setScale(1);

// //    this.input.enableDebug(this.horse, 0xff00ff);

// this.brush = this.add.sprite(40, 40, 'brush');
// this.brush.setInteractive();
// this.brush.setScale(0.1);

// //    this.brushCursor = this.add.sprite(100, 100, 'brush');
// //    this.brushCursor.setScale(0.15);    


// //    this.input.setDraggable(this.brush);

// var particles = this.add.particles('brush');

// this.emitter = particles.createEmitter({
//     x: 0,
//     y: 0,
//     lifespan: 3000,
//     scale: 0.1,
//     speed: { min: 10, max: 30 },
//     frequency: 250,
//     on: false,
//     angle: -45,
//     gravityY: 300
// });

// let brushing = false;
// const self = this;
// this.delta = 0;

// // particle fireworks: https://phaser.io/examples/v3/view/game-objects/particle-emitter/fireworks

// let activityPoints = 0;

// this.brush.on('pointerdown', (pointer, localX, localY, event) => {
//     console.log("brush:pointerdown");
//     this.brushing = true;

//     this.brushCursor = self.add.sprite(40, 40, 'brush');
//     this.brushCursor.setScale(0.15);    
// });

// this.horse.on('pointermove', (pointer, localX, localY, event) => {
//     if (self.brushCursor && pointer.distance > 100) {
//         activityPoints += 1;
//         console.log(activityPoints);
//         if (activityPoints % 25 == 0) {
//             console.log('happy horsey!');
//         }
//     }
// });

// this.horse.on('pointerover', (pointer, localX, localY, event) => {
//     if (this.brushCursor) {
//         this.emitter.startFollow(this.brushCursor);
//         this.emitter.on = true;    
//     }
// });

// this.horse.on('pointerout', function (pointer, event) {
//     if (this.brushCursor) {
//         this.emitter.on = false;    
//     }
// });

// this.input.on('pointermove', function (pointer, gameObject, event) {
//     if (self.brushCursor) {
//         self.brushCursor.x = pointer.x;
//         self.brushCursor.y = pointer.y;
//     }
// });

// this.input.on('pointerup', (pointer, gameObject, event) => {
//     if (self.brushCursor) {
//         self.brushCursor.destroy();
//         activityPoints = 0;
//     }
// });