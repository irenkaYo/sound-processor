SoundProcessor

Консольное приложение для обработки аудиофайлов WAV. Принимает входной файл, пропускает его через цепочку фильтров и записывает результат.


Как запустить

SoundProcessor [-i input.wav] -o output.wav [-f filter params...]


-i — входной WAV-файл (необязательно, если используется генератор)
-o — выходной файл (обязательно)
-f — фильтр с параметрами (можно указывать несколько раз)


Примеры:

SoundProcessor -i input.wav -o output.wav -f ampl 0.8
SoundProcessor -i input.wav -o output.wav -f lowpass 11 -f normalize
SoundProcessor -o output.wav -f generator sin 440 3000


Фильтры

ФильтрПараметрыОписаниеamplfactorУмножает амплитуду на коэффициент (≥ 0)normalize[peak]Нормализует громкость до указанного пика (0–1), по умолчанию 1silenceunit start endВставляет тишину в диапазон (unit: ms или sec)timestretchfactorРастягивает или сжимает звук по времени (> 0)lowpasswindowSizeФильтр нижних частот — скользящее среднее (нечётный размер окна)generator sinfreq_hz duration_msГенерирует синусоиду заданной частоты и длительностиgenerator amamplitude carrier_hz modulation_hz depth duration_msAM-синтез (амплитудная модуляция)generator fmamplitude carrier_hz modulation_hz deviation_hz duration_msFM-синтез (частотная модуляция)


Архитектура

Приложение построено по компонентной архитектуре:


ArgsParser — разбирает аргументы командной строки
CmdLineArgs2PipelineConverter — преобразует распарсенные аргументы в цепочку фильтров
Pipeline — последовательно применяет фильтры к звуковому фрагменту
WavReader / WavWriter — читают и записывают PCM WAV-файлы (16-bit, mono)
Waveform — внутреннее представление звука (список short-сэмплов + метаданные)
IFilter — интерфейс, который реализует каждый фильтр


Фильтры делятся на два типа: эффекты (обрабатывают существующий сигнал) и генераторы (заменяют сигнал новым). Добавить новый фильтр можно без изменения остального кода — достаточно реализовать IFilter и зарегистрировать его в Application.cs.


Поддерживаемый формат WAV


PCM (без сжатия)
16-bit, mono
Частота дискретизации: любая (по умолчанию 44100 Гц)
