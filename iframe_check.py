import io,re,glob,os
expect={'Act2Miniboss.cs':['attackDamage']+['collisionDamage, playerInvulnerability']*2,
 'BeamProjectile.cs':['damage, playerInvulnerability'],'Minion.cs':['damage, playerInvulnerability'],
 'UltProjectile.cs':['damage, playerInvulnerability'],
 'Act1Boss.cs':['aoeDamage, 0.3f','contactDamage, contactInvulnerability'],
 'BeamCharge.cs':['30, 0.5f'],'BeamExtend.cs':['30, 0.5f'],'FinalBoss.cs':['damage, 0.1f'],
 'FireExplosion.cs':['50, 0.5f'],'GroundImpact.cs':['50, 0.5f'],'Laser.cs':['10, 0.05f'],
 'LightningBolt.cs':['damage, 0.05f'],'MissileAttack.cs':['30, 0.1f'],'Shockwave.cs':['50, 0.5f'],
 'WindBlast.cs':['damage, 0.05f'],'Bullet.cs':['damage, hitFlash'],
 'Mob.cs':['contactDamage, contactInvulnerability']}
got={}
for f in glob.glob('**/*.cs',recursive=True):
    a=re.findall(r'\.CharacterAttacked\(([^;]+)\);',io.open(f,encoding='utf-8').read())
    if a: got[os.path.basename(f)]=[x.strip() for x in a]
bad=[k for k,v in expect.items() if got.get(k)!=v]
for k in bad: print('MISMATCH',k,'expected',expect[k],'got',got.get(k))
assert not bad,'%d call sites lost their i-frame duration'%len(bad)
assert 'GrantInvulnerability' not in io.open('Entities/Mobs/Mob.cs',encoding='utf-8').read()
for f in ['Entities/Mobs/Mob.cs','Entities/Characters/Character.cs','Entities/Characters/Companion.cs']:
    s=io.open(f,encoding='utf-8').read()
    assert s.count('{')==s.count('}'),f+' braces unbalanced'
print('OK: %d call sites kept their durations; braces balanced'%sum(len(v) for v in expect.values()))
