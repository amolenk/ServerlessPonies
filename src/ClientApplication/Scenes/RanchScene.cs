using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using ClientApplication;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class RanchScene : Scene
    {
        public const string Name = "Ranch";

        private static readonly List<Point[]> EnclosureHitboxes = new List<Point[]>
        {
            new Point[] { new Point(80, 110), new Point(265, 20), new Point(395, 275), new Point(218, 365) },
            new Point[] { new Point(405, 0), new Point(612, 0), new Point(623, 280), new Point(422, 290) },
            new Point[] { new Point(738, 70), new Point(953, 190), new Point(813, 392), new Point(640, 281) }
        };

        private static readonly Dictionary<string, Point> AnimalPositions = new Dictionary<string, Point>
        {
            { "1", new Point(250, 195) },
            { "2", new Point(500, 145) },
            { "3", new Point(792, 235) }
        };

        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop =>
            {
                interop
                    .AddSprite("sprBackground", "backgrounds/ranch", 640, 512)
                        .OnPointerUp(nameof(sprBackground_PointerUp));

                foreach (var animal in State.Animals)
                {
                    if (animal.EnclosureName != null)
                    {
                        AddAnimalSprite(animal);
                    }

                }
            });
        }

        [JSInvokable]
        public void sprBackground_PointerUp(SpritePointerEventArgs e)
        {
            if (TryGetEnclosureAtPosition((int)e.X, (int)e.Y, out string clickedEnclosureName))
            {
                State.SelectedEnclosureName = clickedEnclosureName;
                Phaser(interop => interop.SwitchToScene(AnimalManagementScene.Name));
            }
        }

        [JSInvokable]
        public void Animal_PointerUp(SpritePointerEventArgs e)
        {
            State.SelectedAnimalName = SpriteName.ExtractId(e.SpriteName);

            Phaser(interop => interop.SwitchToScene(AnimalCareScene.Name));
        }

        protected override void WireStateHandlers(GameState state)
        {
            foreach (var animal in state.Animals)
            {
                animal.EnclosureChanged += AnimalEnclosureChanged;
            }
        }

        private void AnimalEnclosureChanged(object sender, EnclosureChangedEventArgs e)
        {
            var animal = (Animal)sender;

            Phaser(interop =>
            {
                var animalSpriteName = SpriteName.Create("animal", animal.Name);
                var animalSprite = interop.Sprite(animalSpriteName);
                
                if (e.EnclosureName == null)
                {
                    if (animalSprite.Exists())
                    {
                        interop.RemoveSprite(animalSpriteName);
                    }
                }
                else
                {
                    var animalPosition = AnimalPositions[animal.EnclosureName];

                    if (animalSprite.Exists())
                    {
                        animalSprite.Move(animalPosition.X, animalPosition.Y);
                    }
                    else
                    {
                        interop.AddSprite(animalSpriteName, $"animals/{animal.Name}/top",
                            animalPosition.X, animalPosition.Y, options => options
                                .OnPointerUp(nameof(Animal_PointerUp)));
                    }
                }
            });
        }

        private void AddAnimalSprite(Animal animal)
        {
            Phaser(interop =>
            {
                var animalSpriteName = SpriteName.Create("animal", animal.Name);
                var animalPosition = AnimalPositions[animal.EnclosureName];

                interop
                    .AddSprite(animalSpriteName, $"animals/{animal.Name}/top",
                        animalPosition.X, animalPosition.Y, options => options
                            .OnPointerUp(nameof(Animal_PointerUp)));
            });
        }

        private bool TryGetEnclosureAtPosition(int x, int y, out string enclosureName)
        {
            for (var i = 0; i < EnclosureHitboxes.Count; i++)
            {
                if (IsInPolygon(EnclosureHitboxes[i], new Point(x, y)))
                {
                    enclosureName = (i + 1).ToString();
                    return true;
                }
            }
            enclosureName = string.Empty;
            return false;
        }

        private static bool IsInPolygon(Point[] poly, Point p)
        {
            Point p1, p2;
            bool inside = false;

            if (poly.Length < 3)
            {
                return inside;
            }

            var oldPoint = new Point(
                poly[poly.Length - 1].X, poly[poly.Length - 1].Y);

            for (int i = 0; i < poly.Length; i++)
            {
                var newPoint = new Point(poly[i].X, poly[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                    && (p.Y - (long) p1.Y)*(p2.X - p1.X)
                    < (p2.Y - (long) p1.Y)*(p.X - p1.X))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }
    }
}