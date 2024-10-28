using GDWeave;
using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace NoAccessoryLimit;



public class Mod : IMod {
    public Config Config;
    
    public Mod(IModInterface modInterface) {
        int limit = -1;
        this.Config = modInterface.ReadConfig<Config>();
        if (Config != null)
            limit = Config.AccessoryLimit;
        modInterface.Logger.Information("Accessory Limit Set to: " + (limit == -1 ? "Infinite" : limit.ToString()));
        modInterface.RegisterScriptMod(new ScriptMod(limit));
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}

public class ScriptMod : IScriptMod
{
    public int limit = -1;

    public ScriptMod(int _limit)
    {
        limit = _limit;
    }
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var waiter = new MultiTokenWaiter([
            t => t.Type is TokenType.OpLess,
            t => t is ConstantToken { Value: IntVariant { Value: 4} }
        ], allowPartialMatch:false);

        foreach (var token in tokens)
        {
            if (waiter.Check(token))
            {
                yield return new ConstantToken(new IntVariant(limit == -1 ? int.MaxValue : limit));
            }
            else
                yield return token;
        }
    }
}