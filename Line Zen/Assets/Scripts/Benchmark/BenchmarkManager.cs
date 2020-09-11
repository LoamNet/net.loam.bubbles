using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System;

// Base class
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

public enum BenchmarkState
{
    // One-time Setup
    Idle,  // Passive
    Begin, // Spoolup


    // Specific loop ber bench
    Init,    // Set up the data for the run.
    Execute,
    Display,


    // Done, reset allowed.
    Finished,
}

public class BenchmarkManager : MonoBehaviour
{
    public Button run;
    public BenchmarkDisplay display;
    public TMPro.TextMeshProUGUI progress;

    private List<Benchmark> benchmarks;
    private List<float> times;
    private BenchmarkState state;
    private Benchmark currentBench;
    private int currentBenchIndex = 0;
    private long startTime;
    private long endTime;
    private Utils.WichmannRng rand;
    private const int BENCH_GENERAL_SEED = 28282;

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
            // General states
            case BenchmarkState.Idle:
                break;

            case BenchmarkState.Begin:
                benchmarks = new List<Benchmark>();
                System.Random sysRand = new System.Random(); // Derived by default from system clock. Can be different per system, but it's effectively random.
                rand = new Utils.WichmannRng(BENCH_GENERAL_SEED);

                // Prevent const folding w/ sys rand, use wichmannRNG for controlled rand in testing
                double sysDouble = sysRand.NextDouble() * 2;
                Debug.Log($"Sys double: {sysDouble}");
                if (sysDouble >= 1)
                {
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
                }


                currentBench = benchmarks[currentBenchIndex];
                run.interactable = false;
                state = BenchmarkState.Init;
                break;




            // Specific states
            case BenchmarkState.Init:
                times = new List<float>(100);
                state = BenchmarkState.Execute;
                progress.text = "";
                System.GC.Collect(generation: 0, mode: GCCollectionMode.Forced, blocking: true);

                // Begin recording right before coroutine start
                startTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // At start of execute
                StartCoroutine(currentBench.Perform(
                    () =>
                    {
                        endTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        state = BenchmarkState.Display;
                    },
                    (percent) =>
                    {
                        progress.text = $"{percent} %";
                    }
                ));
                break;

            case BenchmarkState.Execute:
                times.Add(FPS.CurrentFPS);
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
public class Benchmark_Double : Benchmark
{
    private double accum;

    public Benchmark_Double(Utils.WichmannRng rand, string name) : base(rand, name)
    { }

    public override IEnumerator Perform(Action onFinish, Action<int> oneInTwentyStep)
    {
        accum = rand.Next();
        const double num = 100000000.0d;
        const int mod = (((int)num) / 20);

        for (double i = 0; i < num; i += 1.0d)
        {
            accum *= .99d;
            accum *= 1.006d;
            accum /= 0.996d;
            accum += 12.3d;
            accum *= .99d;
            accum *= 1.006d;
            accum -= accum * .001d;
            accum /= 0.996d;
            accum += 12.3d;
            accum -= accum * .001d;

            if (i % mod == 0)
            {
                oneInTwentyStep?.Invoke((int)(i / num * 100.0d));
                yield return null;
            }
        }

        onFinish();
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
        const float num = 100000000.0f;
        const int mod = (((int)num) / 20);

        for (float i = 0; i < num; i += 1.0f)
        {
            accum *= .99f;
            accum *= 1.01f;
            accum /= 0.996f;
            accum += 12.3f;
            accum *= .99f;
            accum *= 1.01f;
            accum -= accum * .001f;
            accum /= 0.996f;
            accum += 12.3f;
            accum -= accum * .001f;

            if (i % mod == 0)
            {
                oneInTwentyStep?.Invoke((int)(i / num * 100.0f));
                yield return null;
            }
        }

        onFinish();
    }
}