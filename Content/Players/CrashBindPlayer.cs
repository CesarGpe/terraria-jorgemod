using eslamio.Common;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace eslamio.Content.Players;

[Autoload(Side = ModSide.Client)]
public class CrashbindPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (KeybindSystem.CrashGameKey.JustPressed && !Main.dedServ)
        {
            Main.NewText("Era un día como cualquier otro solo que llovía sangre, no le tome importancia asi que fui al tianguis de mi tío. Mi tío vende productos audiovisuales de gente haciendo el amor. Yo fui para buscar lo nuevo de Alexis Texas y Abella Danger, estaba todo agotado para mí sorpresa. Entonces mi tío se me acerca y me dice que uno de sus clientes le regreso un DVD. Así que me lo regala, en eso me percató que el DVD tiene un nombre muy extraño, decía: \"el pepe ete sech\". Se me hizo extraño, pero pensé que era un producto audiovisual de femboys haciendo el amor así que me lo lleve de todas formas hacia mi casa para reproducirlo en mi televisor sacado de coppel a 12 meses. Cuando le doy al play me percato que solo son unas palabras en pantalla que decían: \"yo soy el pepe, yo soy el ete sech, yo soy Dios\". Justo cuando termine de leer ese texto que estaba reproduciendo mi televisor, surge un cortocircuito y en eso una figura grande y oscura sale de mi televisor, se me acerca poco a poco con una mirada muy penetrante, tenía los ojos como de un demonio, no lo podía creer me quedé en shock, no podía moverme. Con una voz muy seductora, varonil y erótica me dice: \"yo soy el pepe ete sech\". En eso mi dopamina aumenta de forma exagerada, tanto que mi erección se vuelve mortal y después me dió diabetes tipo 2, tuve que cortar mis piernas para regular mi sangre. En eso el pepe ete sech me dice: \"para que te deje en paz tienes que darme tu alma\". No me quedo de otra, tuve que dar mi alma. Actualmente vago en las calles de ciudad Juárez sin alma, como un fantasma... Pero no le tome importancia. ", Color.Red);
            Main.WeGameRequireExitGame();
        }
    }
}