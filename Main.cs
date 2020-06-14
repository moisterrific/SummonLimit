using System;
using System.Reflection;
using System.Timers;
using Terraria;
using TerrariaApi.Server;

namespace SummonLimit
{
  [ApiVersion(2, 1)]
  public partial class SummonLimit : TerrariaPlugin
  {
    /// <summary>
    ///   <para>
    ///     The amount of maximum summons available without
    ///     hacking or modding.
    ///   </para>
    ///   This value is
    ///   <value>11</value>
    ///   as of Terraria 1.4.0.5.
    /// </summary>
    internal const ushort MaxSummons = 11;

    /// <summary>
    ///   The root permission for the plugin.
    /// </summary>
    internal const string Permission = "summon.limit";

    /// <summary>
    ///   Message used for kicking the player.
    /// </summary>
    internal const string KickMessage = "You have exceeded the max amount of allowed minions.";

    /// <summary>
    ///   <see cref="Timer" /> used for performing minion checks
    ///   and cleanup operations.
    /// </summary>
    internal static readonly Timer Metronome = new Timer(3000);

    public SummonLimit(Main game) : base(game)
    {
    }

    public override void Initialize()
    {
      ServerApi.Hooks.GamePostInitialize.Register(this, OnPostInitialize);
    }

    protected override void Dispose(bool disposing)
    {
      Warned.Clear();

      Metronome.Elapsed -= Check;
      Metronome.Elapsed -= CleanWarned;
      Metronome.Stop();
      Metronome.Dispose();

      ServerApi.Hooks.GamePostInitialize.Deregister(this, OnPostInitialize);

      base.Dispose(disposing);
    }

    private static void OnPostInitialize(EventArgs e)
    {
      Metronome.Elapsed += Check;
      Metronome.Elapsed += CleanWarned;
      Metronome.Start();
    }

    #region Meta

    public override string Name => "SummonLimit";
    public override string Description => "Prevents summon hacking.";
    public override string Author => "Newy";
    public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

    #endregion
  }
}
