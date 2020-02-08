const sceneInfos = [];
let currentScene;

function findSprite(sceneName, spriteName) {
    const sceneInfo = sceneInfos[sceneName];
    return sceneInfo.phaserScene.children.list.find(child => {
        return child.type === "Sprite" && child.name === spriteName
    });
}

function addSpriteEventHandler(sceneName, spriteName, eventName, handlerName) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.setInteractive({ pixelPerfect: true });
    sprite.on(eventName, (pointer) => {
        const sceneInfo = sceneInfos[sceneName];
        const eventArgs = { 'spriteName': spriteName, 'x': pointer.x, 'y': pointer.y, 'distance': -1 };
        if (pointer.distance) eventArgs.distance = pointer.distance;
        sceneInfo.dotNetScene.invokeMethodAsync(handlerName, eventArgs);
    });
}

function addSceneEventHandler(sceneName, eventName, handlerName) {
    const sceneInfo = sceneInfos[sceneName];
    sceneInfo.phaserScene.input.on(eventName, (pointer) => {
        const eventArgs = { 'x': pointer.x, 'y': pointer.y, 'distance': -1 };
        //if (pointer.distance) eventArgs.distance = pointer.distance;
        sceneInfo.dotNetScene.invokeMethod(handlerName, eventArgs);
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

function setSpriteLocation(sceneName, spriteName, x, y) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.x = x;
    sprite.y = y;
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

function removeSprite(sceneName, spriteName) {
    const sprite = findSprite(sceneName, spriteName);
    if (sprite) {
        sprite.destroy();
    }
}

function addRectangle(sceneName, x, y, width, height, color) {

    const sceneInfo = sceneInfos[sceneName];
    var graphics = sceneInfo.phaserScene.add.graphics({ fillStyle: { color: color } });

    var rect = new Phaser.Geom.Rectangle(x, y, width, height);

    graphics.fillRectShape(rect);
}

function isSceneVisible(scene) {
    console.log('scene active: ' + scene + ' = ' + game.scene.isActive(scene));
    console.log('scene visible: ' + scene + ' = ' + game.scene.isVisible(scene));
    return game.scene.isActive(scene)
        || sceneInfos[scene].isCreating;
}

function switchScene(from, to) {
    console.log('stopping scene: ' + from);
    game.scene.stop(from);
    console.log('starting scene: ' + to);
    game.scene.start(to);
    console.log('scene active after start: ' + to + ' = ' + game.scene.isActive(to));
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
        sceneInfos[name].isCreating = true;
        dotNetScene.invokeMethod('create');
        sceneInfos[name].isCreating = false;
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