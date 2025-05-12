using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BeatManager : MonoBehaviour
{
    public List<TroncoPatternData> troncosDisponiveis;
    private TroncoPatternData troncoAtual;

    public float timingWindow = 0.3f;

    public AudioClip guiaSound;
    public AudioClip machadoSound;
    public AudioClip erroSound;
    public AudioSource audioSource;

    public Image beatIndicatorImage;
    public Color corInativa = Color.white;
    public Color corAtiva = Color.green;
    public TMP_Text indicadorTexto;

    public VidaManager vidaManager;

    private enum Estado { Guia, Execucao }
    private Estado estadoAtual;

    private float songStartTime;
    private int currentBeat = 0;

    private bool podeAcertar = false;
    private float janelaInicio, janelaFim;

    private float[] beatTimings;
    private List<float> execucaoTempos = new List<float>();

    private int tentativas = 0;

    void Start()
    {
        CarregarNovoTronco();
    }

    void Update()
    {
        float currentTime = Time.time - songStartTime;

        switch (estadoAtual)
        {
            case Estado.Guia:
                if (currentBeat < beatTimings.Length)
                {
                    if (currentTime >= beatTimings[currentBeat])
                    {
                        audioSource.PlayOneShot(guiaSound);
                        currentBeat++;
                    }
                }
                else
                {
                    // Inicia a execução
                    estadoAtual = Estado.Execucao;
                    currentBeat = 0;
                    songStartTime = Time.time;
                    execucaoTempos.Clear();
                }
                break;

            case Estado.Execucao:
                if (currentBeat < beatTimings.Length)
                {
                    float beatTime = beatTimings[currentBeat];

                    if (!podeAcertar && currentTime >= beatTime - timingWindow)
                    {
                        podeAcertar = true;
                        janelaInicio = beatTime - timingWindow;
                        janelaFim = beatTime + timingWindow;
                        beatIndicatorImage.color = corAtiva;
                        indicadorTexto.color = corAtiva;
                    }

                    if (podeAcertar && currentTime >= janelaFim)
                    {
                        // Errou por não clicar
                        // audioSource.PlayOneShot(erroSound);
                        vidaManager.PerderVida();
                        podeAcertar = false;
                        beatIndicatorImage.color = corInativa;
                        indicadorTexto.color = corInativa;
                        currentBeat++;
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (currentTime >= janelaInicio && currentTime <= janelaFim)
                        {
                            audioSource.PlayOneShot(machadoSound);
                        }
                        else
                        {
                            audioSource.PlayOneShot(erroSound);
                            vidaManager.PerderVida();
                        }

                        podeAcertar = false;
                        beatIndicatorImage.color = corInativa;
                        indicadorTexto.color = corInativa;
                        currentBeat++;
                    }
                }
                else
                {
                    // Verifica se acertou tudo
                    if (vidaManager.vidas > 0)
                        CarregarNovoTronco();
                }
                break;
        }
    }

    void CarregarNovoTronco()
    {
        troncoAtual = troncosDisponiveis[Random.Range(0, troncosDisponiveis.Count)];
        beatTimings = new float[troncoAtual.beatTimings.Length];

        // Ajusta tempos relativos ao início
        float acumulador = 1f; // Delay inicial
        for (int i = 0; i < troncoAtual.beatTimings.Length; i++)
        {
            acumulador += troncoAtual.beatTimings[i];
            beatTimings[i] = acumulador;
        }

        currentBeat = 0;
        estadoAtual = Estado.Guia;
        songStartTime = Time.time;

        beatIndicatorImage.color = corInativa;
        indicadorTexto.color = corInativa;
    }
}
