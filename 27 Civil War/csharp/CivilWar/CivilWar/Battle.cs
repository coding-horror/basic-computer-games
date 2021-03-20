using System;
using System.Collections.Generic;

namespace CivilWar
{
    public enum Side { Confederate, Union, Both }
    public enum Option { Battle, Replay, Quit }

    public record Battle(string Name, int[] Men, int[] Casualties, Side Offensive, string Description)
    {
        public static readonly List<Battle> Historic = new()
        {
            new("Bull Run", new[] { 18000, 18500 }, new[] { 1967, 2708 }, Side.Union, "July 21, 1861.  Gen. Beauregard, commanding the south, met Union forces with Gen. McDowell in a premature battle at Bull Run. Gen. Jackson helped push back the union attack."),
            new("Shiloh", new[] { 40000, 44894 }, new[] { 10699, 13047 }, Side.Both, "April 6-7, 1862.  The confederate surprise attack at Shiloh failed due to poor organization."),
            new("Seven Days", new[] { 95000, 115000 }, new[] { 20614, 15849 }, Side.Both, "June 25-july 1, 1862.  General Lee (csa) upheld the offensive throughout the battle and forced Gen. McClellan and the union forces away from Richmond."),
            new("Second Bull Run", new[] { 54000, 63000 }, new[] { 10000, 14000 }, Side.Confederate, "Aug 29-30, 1862.  The combined confederate forces under Lee and Jackson drove the union forces back into Washington."),
            new("Antietam", new[] { 40000, 50000 }, new[] { 10000, 12000 }, Side.Both, "Sept 17, 1862.  The south failed to incorporate Maryland into the confederacy."),
            new("Fredericksburg", new[] { 75000, 120000 }, new[] { 5377, 12653 }, Side.Union, "Dec 13, 1862.  The confederacy under Lee successfully repulsed an attack by the union under Gen. Burnside."),
            new("Murfreesboro", new[] { 38000, 45000 }, new[] { 11000, 12000 }, Side.Union, "Dec 31, 1862.  The south under Gen. Bragg won a close battle."),
            new("Chancellorsville", new[] { 32000, 90000 }, new[] { 13000, 17197 }, Side.Confederate, "May 1-6, 1863.  The south had a costly victory and lost one of their outstanding generals, 'stonewall' Jackson."),
            new("Vicksburg", new[] { 50000, 70000 }, new[] { 12000, 19000 }, Side.Union, "July 4, 1863.  Vicksburg was a costly defeat for the south because it gave the union access to the Mississippi."),
            new("Gettysburg", new[] { 72500, 85000 }, new[] { 20000, 23000 }, Side.Both, "July 1-3, 1863.  A southern mistake by Gen. Lee at Gettysburg cost them one of the most crucial battles of the war."),
            new("Chickamauga", new[] { 66000, 60000 }, new[] { 18000, 16000 }, Side.Confederate, "Sept. 15, 1863. Confusion in a forest near Chickamauga led to a costly southern victory."),
            new("Chattanooga", new[] { 37000, 60000 }, new[] { 36700, 5800 }, Side.Confederate, "Nov. 25, 1863. After the south had sieged Gen. Rosencrans’ army for three months, Gen. Grant broke the siege."),
            new("Spotsylvania", new[] { 62000, 110000 }, new[] { 17723, 18000 }, Side.Confederate, "May 5, 1864.  Grant's plan to keep Lee isolated began to fail here, and continued at Cold Harbor and Petersburg."),
            new("Atlanta", new[] { 65000, 100000 }, new[] { 8500, 3700 }, Side.Union, "August, 1864.  Sherman and three veteran armies converged on Atlanta and dealt the death blow to the confederacy."),
        };

        public static (Option, Battle?) SelectBattle()
        {
            Console.WriteLine("\n\n\nWhich battle do you wish to simulate?");
            return int.Parse(Console.ReadLine() ?? "") switch
            {
                0 => (Option.Replay, null),
                >0 and <15 and int n  => (Option.Battle, Historic[n-1]),
                _ => (Option.Quit, null)
            };
        }
    }
}
