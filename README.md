SoundProcessor

A console application for processing WAV audio files. It accepts an input file, runs it through a filter chain, and writes the result.

The project is written in C# (.NET 9).

How to run

SoundProcessor [-i input.wav] -o output.wav [-f filter params...]

-i — input WAV file (optional if using a generator)
-o — output file (required)
-f — filter with parameters (can be specified multiple times)

Examples:

SoundProcessor -i input.wav -o output.wav -f ampl 0.8
SoundProcessor -i input.wav -o output.wav -f lowpass 11 -f normalize
SoundProcessor -o output.wav -f generator sin 440 3000

Filters

Filter Parameters Description amplitude factor multiplies the amplitude by a factor (≥ 0) normalize[peak] normalizes the volume to the specified peak (0–1), defaults to 1 silenceunit start end inserts silence in a range (unit: ms or sec)timestretchfactor: Stretches or compresses audio over time (> 0); lowpasswindowSize: Low-pass filter — moving average (odd window size); generatorsinfreq_hz: duration_ms: Generates a sine wave of a specified frequency and duration; generatorsamplitudecarrier_hz: modulation_hz: depthduration_ms: AM synthesis (amplitude modulation); generatorsamplitudecarrier_hz: modulation_hz: deviation_hz: duration_ms: FM synthesis (frequency modulation)

Architecture

The application is built using a component architecture:

ArgsParser: Parses command-line arguments
CmdLineArgs2PipelineConverter: Converts parsed arguments into a filter chain
Pipeline: Sequentially applies filters to an audio fragment
WavReader / WavWriter: Read and write PCM WAV files (16-bit, mono)
Waveform — the internal representation of sound (list of short samples + metadata)
IFilter — the interface that each filter implements

Filters are divided into two types: effects (process an existing signal) and generators (replace a signal with a new one). You can add a new filter without changing the rest of the code—simply implement the IFilter and register it in Application.cs.

Supported format: WAV

PCM (uncompressed)
16-bit, mono
Sampling rate: any (default: 44100 Hz)
