using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System;

// Base class for benchmarks, below
public abstract class Benchmark
{
    public string Name { get; private set; }
    protected Utils.WichmannRng rand;
    public Benchmark(Utils.WichmannRng rand, string name)
    {
        this.Name = name;
        this.rand = rand;
    }

    // Call until false is returned
    public abstract IEnumerator Perform(System.Action onFinish, System.Action<int> oneInTwentyStep);
}

// Staaaaaaaaaate machine
public enum BenchmarkState
{
    // One-time Setup
    Idle,  // Passive
    Begin, // Spool up


    // Specific loop ber bench
    Init,    // Set up the data for the run.
    Execute, // Passive, only timed area
    Display, // Spool down


    // Done, reset allowed.
    Finished,
}

public class BenchmarkManager : MonoBehaviour
{
    // Ext display / interaction
    public Button run;
    public BenchmarkDisplay display;
    public TMPro.TextMeshProUGUI progress;

    // Internal
    private List<Benchmark> benchmarks;
    private BenchmarkState state;
    private Benchmark currentBench;
    private int currentBenchIndex = 0;
    private long startTime;
    private long endTime;
    private Utils.WichmannRng rand;
    private const int BENCH_GENERAL_SEED = 28282;
    private string preformat = "";

    // Start is called before the first frame update
    void Start()
    {
        progress.text = "";
        run.onClick.AddListener(() => state = BenchmarkState.Begin);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            // General idle states
            case BenchmarkState.Idle:
                // This state intentionally left blank
                break;

            // Run once to set up the benchmark. NOTE: Avoiding const folding chance w/ system random to choose specific seeded random.
            case BenchmarkState.Begin:
                benchmarks = new List<Benchmark>();
                System.Random sysRand = new System.Random(); // Derived by default from system clock. Can be different per system, but it's effectively random.
                rand = new Utils.WichmannRng(BENCH_GENERAL_SEED);

                // Prevent const folding w/ sys rand, use wichmannRNG for controlled rand in testing
                double sysDouble = sysRand.NextDouble() * 2;
                Debug.Log($"Sys double: {sysDouble}");
                if (sysDouble >= 1)
                {
                    benchmarks.Add(new Benchmark_Int(rand, "I ops"));
                    benchmarks.Add(new Benchmark_Int(rand, "I ops"));
                    benchmarks.Add(new Benchmark_Long(rand, "L ops"));
                    benchmarks.Add(new Benchmark_Int(rand, "I ops"));
                    benchmarks.Add(new Benchmark_Long(rand, "L ops"));
                    benchmarks.Add(new Benchmark_Long(rand, "L ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B D ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B D ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B F ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B D ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B F ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B F ops"));
                }
                else
                {
                    benchmarks.Add(new Benchmark_Double(rand, "1B F ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B F ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B D ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B D ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B F ops"));
                    benchmarks.Add(new Benchmark_Double(rand, "1B D ops"));
                    benchmarks.Add(new Benchmark_Long(rand, "L ops"));
                    benchmarks.Add(new Benchmark_Long(rand, "L ops"));
                    benchmarks.Add(new Benchmark_Int(rand, "I ops"));
                    benchmarks.Add(new Benchmark_Int(rand, "I ops"));
                    benchmarks.Add(new Benchmark_Long(rand, "L ops"));
                    benchmarks.Add(new Benchmark_Int(rand, "I ops"));
                }


                currentBench = benchmarks[currentBenchIndex];
                run.interactable = false;
                state = BenchmarkState.Init;
                break;





            case BenchmarkState.Init:
                // Some casual resets. pre-calc/format as much as possible.
                state = BenchmarkState.Execute;
                preformat = $"%, {currentBenchIndex}/{benchmarks.Count}";
                progress.text = "";

                // Force a blocking clear for the GC to level the playing field
                System.GC.Collect(generation: 0, mode: GCCollectionMode.Forced, blocking: true);

                // Begin recording right before coroutine start. *nothing* after this but coroutine start.
                startTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // At start of execute
                StartCoroutine(currentBench.Perform(
                    () =>
                    {
                        // End time before *anything* else.
                        endTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        state = BenchmarkState.Display;
                    },
                    (percent) =>
                    {
                        // Preformatted for simple join. String.Join is fastest w/o string builder apparently(?) - minimize perf overhead
                        // https://dotnetcoretutorials.com/2020/02/06/performance-of-string-concatenation-in-c/
                        progress.text = String.Join("", percent, preformat);
                    }
                ));
                break;

            case BenchmarkState.Execute:
                // This state intentionally left blank
                break;

            case BenchmarkState.Display:
                long diff = endTime - startTime;
                display.AddItem(currentBench.Name, $"{diff}ms");
                progress.text = "";

                if (++currentBenchIndex < benchmarks.Count)
                {
                    currentBench = benchmarks[currentBenchIndex];
                    state = BenchmarkState.Init;
                }
                else
                {
                    state = BenchmarkState.Finished;
                }
                break;





            // General end state
            case BenchmarkState.Finished:
                run.interactable = true;
                currentBench = null;
                currentBenchIndex = 0;
                state = BenchmarkState.Idle;
                break;
        }
    }
}

public class Benchmark_Int : Benchmark
{
    private int accum;

    public Benchmark_Int(Utils.WichmannRng rand, string name) : base(rand, name)
    { }

    public override IEnumerator Perform(Action onFinish, Action<int> oneInTwentyStep)
    {
        // Done beforehand to avoid extra cost
        accum = (int)(rand.Next() * 100);
        int mult1 = (int)(rand.Next() * 100);
        int mult2 = (int)(rand.Next() * 100);
        int mult3 = (int)(rand.Next() * 100);
        int mult4 = (int)(rand.Next() * 100);
        int div1 = (int)(rand.Next() * 100);
        int div2 = (int)(rand.Next() * 100);
        int arb1 = (int)(rand.Next() * 100);
        int arb2 = (int)(rand.Next() * 100);
        int arb3 = (int)(rand.Next() * 100);
        int arb4 = (int)(rand.Next() * 100);
        int arb5 = (int)(rand.Next() * 100);

        // Encourage fold
        int num = 100000000;
        int mod = (((int)num) / 20);

        for (int i = 0; i < num; i += 1)
        {
            accum *= mult1;
            accum /= div1;
            accum *= mult2;
            accum /= div2;
            accum += arb1;
            accum *= mult3;
            accum *= mult4;
            accum -= arb3;
            accum += arb4;
            accum -= arb5;

            if (i % mod == 0)
            {
                oneInTwentyStep?.Invoke((int)(i / num * 100.0f));
                yield return null;
            }
        }

        onFinish();
        Debug.Log(accum);
    }
}

public class Benchmark_Long : Benchmark
{
    private long accum;

    public Benchmark_Long(Utils.WichmannRng rand, string name) : base(rand, name)
    { }

    public override IEnumerator Perform(Action onFinish, Action<int> oneInTwentyStep)
    {
        // Done beforehand to avoid extra cost
        accum = (int)(rand.Next() * 100);
        long mult1 = (long)(rand.Next() * 100);
        long mult2 = (long)(rand.Next() * 100);
        long mult3 = (long)(rand.Next() * 100);
        long mult4 = (long)(rand.Next() * 100);
        long div1 = (long)(rand.Next() * 100);
        long div2 = (long)(rand.Next() * 100);
        long arb1 = (long)(rand.Next() * 100);
        long arb2 = (long)(rand.Next() * 100);
        long arb3 = (long)(rand.Next() * 100);
        long arb4 = (long)(rand.Next() * 100);
        long arb5 = (long)(rand.Next() * 100);

        // Encourage fold
        const long num = 100000000;
        const int mod = (((int)num) / 20); // Permissable int

        for (long i = 0; i < num; i += 1)
        {
            accum *= mult1;
            accum /= div1;
            accum *= mult2;
            accum /= div2;
            accum += arb1;
            accum *= mult3;
            accum *= mult4;
            accum -= arb3;
            accum += arb4;
            accum -= arb5;

            if (i % mod == 0)
            {
                // callback, minimize
                oneInTwentyStep?.Invoke((int)(i / num * 100.0f));
                yield return null;
            }
        }

        onFinish();
        Debug.Log(accum);
    }
}

public class Benchmark_Double : Benchmark
{
    private double accum;

    public Benchmark_Double(Utils.WichmannRng rand, string name) : base(rand, name)
    { }

    public override IEnumerator Perform(Action onFinish, Action<int> oneInTwentyStep)
    {
        accum = rand.Next();

        double mult1 = (double)(rand.Next() * 10d);
        double mult2 = (double)(rand.Next() * 10d);
        double mult3 = (double)(rand.Next() * 10d);
        double mult4 = (double)(rand.Next() * 10d);
        double div1 = (double)(rand.Next() * 10d);
        double div2 = (double)(rand.Next() * 10d);
        double arb1 = (double)(rand.Next() * 10d);
        double arb2 = (double)(rand.Next() * 10d);
        double arb3 = (double)(rand.Next() * 10d);
        double arb4 = (double)(rand.Next() * 10d);

        // Encourage fold
        const double num = 100000000.0d;
        const int mod = (((int)num) / 20);

        for (double i = 0; i < num; i += 1.0d)
        {
            accum *= mult1;
            accum *= mult2;
            accum /= div1;
            accum += arb1;
            accum *= mult3;
            accum *= mult4;
            accum -= arb2;
            accum /= div2;
            accum += arb3;
            accum -= arb4;

            if (i % mod == 0)
            {
                oneInTwentyStep?.Invoke((int)(i / num * 100.0d));
                yield return null;
            }
        }

        onFinish();
        Debug.Log(accum);
    }
}

public class Benchmark_Float : Benchmark
{
    private float accum;

    public Benchmark_Float(Utils.WichmannRng rand, string name) : base(rand, name)
    { }

    public override IEnumerator Perform(Action onFinish, Action<int> oneInTwentyStep)
    {
        accum = (float)rand.Next();

        float mult1 = (float)(rand.Next() * 10f);
        float mult2 = (float)(rand.Next() * 10f);
        float mult3 = (float)(rand.Next() * 10f);
        float mult4 = (float)(rand.Next() * 10f);
        float div1 = (float)(rand.Next() * 10f);
        float div2 = (float)(rand.Next() * 10f);
        float arb1 = (float)(rand.Next() * 10f);
        float arb2 = (float)(rand.Next() * 10f);
        float arb3 = (float)(rand.Next() * 10f);
        float arb4 = (float)(rand.Next() * 10f);

        // Encourage fold
        const float num = 100000000.0f;
        const int mod = (((int)num) / 20);

        for (float i = 0; i < num; i += 1.0f)
        {
            accum *= mult1;
            accum *= mult2;
            accum /= div1;
            accum += arb1;
            accum *= mult3;
            accum *= mult4;
            accum -= arb2;
            accum /= div2;
            accum += arb3;
            accum -= arb4;

            if (i % mod == 0)
            {
                oneInTwentyStep?.Invoke((int)(i / num * 100.0f));
                yield return null;
            }
        }

        onFinish();
        Debug.Log(accum);
    }
}