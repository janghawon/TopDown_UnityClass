using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Beam : PoolableMono
{
    private VisualEffect _beamMuzzle;
    private VisualEffect _beamFlare;

    private LineRenderer _lineRenderer;
    private Light _beamLight;

    [SerializeField] private float _beamLength = 10f;
    public LayerMask _whatIsEnemy;

    [SerializeField] private float _beamTime = 0.6f;
    [SerializeField] private int _beamDamage = 5;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _beamMuzzle = transform.Find("BeamMuzzle").GetComponent<VisualEffect>();
        _beamLight = transform.Find("BeamMuzzle/BeamLight").GetComponent<Light>();
        _beamFlare = transform.Find("BeamFlare").GetComponent<VisualEffect>();

        _lineRenderer.enabled = false;
        _beamLight.enabled = false;
        _beamMuzzle.Stop();
        _beamFlare.Stop();

        Init();
    }

    public override void Init()
    {
        _lineRenderer.enabled = false;
        _beamLight.enabled = false;
        _beamMuzzle.Stop();
        _beamFlare.Stop();
    }

    public void PreCharging()
    {
        _beamMuzzle.Play();
        _beamLight.enabled = true;
    }

    public void FireBeam(int damage, Vector3 targetDir)
    {
        float r = _lineRenderer.startWidth;
        RaycastHit hit;
        bool isHit = Physics.SphereCast(transform.position, r, targetDir.normalized, out hit, _beamLength);
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        if(isHit)
        {
            _lineRenderer.SetPosition(1, hit.point);
            _beamFlare.transform.position = hit.point;

            if(hit.collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                health.OnDamage(damage, hit.point, hit.normal);
            }
        }
        else
        {
            Vector3 endpos = transform.position + targetDir * _beamLength;
            _lineRenderer.SetPosition(1, endpos);
            _beamFlare.transform.position = endpos;
        }

        _beamFlare.Play();
        StartCoroutine(DelayStop());
    }
    
    public void StopBeam()
    {
        StartCoroutine(StopSequence());
    }

    private IEnumerator DelayStop()
    {
        yield return new WaitForSeconds(_beamTime);
        StopBeam();
    }

    private IEnumerator StopSequence()
    {
        _lineRenderer.enabled = false;
        _beamLight.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _beamMuzzle.Stop();
        _beamFlare.Stop();
        yield return new WaitForSeconds(0.3f);
        PoolManager.Instance.Push(this);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PreCharging();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool result = Physics.Raycast(ray, out hit, Define.MainCam.farClipPlane, 1 << LayerMask.NameToLayer("Ground"));

            Vector3 pos = hit.point;
            pos.y = transform.position.y;
            Vector3 dir = pos - transform.position;
            if(result)
            {
                FireBeam(5, dir);
            }
            
        }
    }
}
