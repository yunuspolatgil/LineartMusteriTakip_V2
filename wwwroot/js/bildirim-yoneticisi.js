/**
 * Bildirim Yönetim Sistemi - Düzeltilmiş Versiyon
 * Gerçek zamanlı bildirim takibi ve yönetimi
 */
class BildirimYoneticisi {
    constructor() {
        this.bildirimler = [];
        this.okunmamisSayisi = 0;
        this.initialized = false;
        this.updateInterval = null;
        this.DEBUG = true;

        this.log('BildirimYoneticisi başlatılıyor...');
    }

    log(message, data = null) {
        if (this.DEBUG) {
            console.log(`[BildirimYoneticisi] ${message}`, data || '');
        }
    }

    /**
     * Bildirim sistemini başlat
     */
    async init() {
        if (this.initialized) {
            this.log('Sistem zaten başlatılmış!');
            return;
        }

        try {
            this.log('Sistem başlatılıyor...');
            
            await this.bildirimleriyukle();
            this.setupEventListeners();
            this.startPeriodicUpdate();
            
            this.initialized = true;
            this.log('Sistem başarıyla başlatıldı!');
            
        } catch (error) {
            console.error('[BildirimYoneticisi] Başlatma hatası:', error);
        }
    }

    setupEventListeners() {
        const dropdown = document.getElementById('notificationDropdown');
        if (dropdown) {
            dropdown.addEventListener('show.bs.dropdown', () => {
                this.bildirimleriyukle();
            });
        }

        document.addEventListener('visibilitychange', () => {
            if (!document.hidden && this.initialized) {
                this.bildirimleriyukle();
            }
        });
    }

    startPeriodicUpdate() {
        if (this.updateInterval) {
            clearInterval(this.updateInterval);
        }

        this.updateInterval = setInterval(() => {
            this.bildirimleriyukle();
        }, 30000);

        this.log('Periyodik güncelleme başlatıldı (30s)');
    }

    async bildirimleriyukle() {
        try {
            this.log('Bildirimler API\'den yükleniyor...');
            
            const response = await fetch('/api/bildirimler');
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            const bildirimler = await response.json();
            this.log('API\'den yüklenen bildirimler:', bildirimler);
            
            this.bildirimler = bildirimler || [];
            this.okunmamisSayisiHesapla();
            this.UIGuncelle();
            
        } catch (error) {
            console.error('[BildirimYoneticisi] Bildirim yükleme hatası:', error);
            this.fallbackBildirimGoster();
        }
    }

    okunmamisSayisiHesapla() {
        this.okunmamisSayisi = this.bildirimler.filter(b => b.durum === 'Okunmamis').length;
        this.log(`Okunmamış bildirim sayısı hesaplandı: ${this.okunmamisSayisi}`);
        this.log('Bildirimler durum kontrolü:', this.bildirimler.map(b => ({ id: b.id, durum: b.durum })));
    }

    UIGuncelle() {
        this.log('UI güncelleniyor...');
        this.badgeGuncelle();
        this.bildirimListesiniGuncelle();
        this.toplamSayiGuncelle();
    }

    badgeGuncelle() {
        const badge = document.getElementById('notificationBadge');
        this.log(`Badge güncelleniyor - Okunmamış sayı: ${this.okunmamisSayisi}`);
        
        if (badge) {
            if (this.okunmamisSayisi > 0) {
                badge.textContent = this.okunmamisSayisi > 99 ? '99+' : this.okunmamisSayisi;
                badge.style.display = 'block';
                this.log(`Badge gösteriliyor: ${badge.textContent}`);
            } else {
                badge.style.display = 'none';
                this.log('Badge gizleniyor');
            }
        } else {
            this.log('Badge element bulunamadı!');
        }
    }

    toplamSayiGuncelle() {
        const totalElement = document.getElementById('totalNotifications');
        if (totalElement) {
            totalElement.textContent = this.bildirimler.length;
        }
    }

    bildirimListesiniGuncelle() {
        const listElement = document.getElementById('notificationsList');
        if (!listElement) return;

        if (this.bildirimler.length === 0) {
            listElement.innerHTML = `
                <li class="list-group-item p-4 text-center text-muted">
                    <i class="ti ti-bell-off mb-2" style="font-size: 2rem;"></i>
                    <p class="mb-0">Henüz bildirim yok</p>
                </li>
            `;
            return;
        }

        const sonBildirimler = this.bildirimler.slice(0, 10);
        listElement.innerHTML = sonBildirimler.map(bildirim => this.bildirimHTMLOlustur(bildirim)).join('');
    }

    bildirimHTMLOlustur(bildirim) {
        const tarih = new Date(bildirim.createDate).toLocaleString('tr-TR');
        const iconClass = this.bildirimIconGetir(bildirim.tur);
        const bgClass = bildirim.durum === 'Okunmamis' ? 'bg-light' : '';
        
        return `
            <li class="list-group-item notification-item ${bgClass}" data-id="${bildirim.id}">
                <div class="d-flex">
                    <div class="flex-shrink-0 me-3">
                        <div class="avatar">
                            <span class="avatar-initial rounded-circle ${this.bildirimRenkGetir(bildirim.tur)}">
                                <i class="${iconClass}"></i>
                            </span>
                        </div>
                    </div>
                    <div class="flex-grow-1">
                        <div class="d-flex justify-content-between align-items-start">
                            <div>
                                <h6 class="mb-1">${bildirim.baslik}</h6>
                                <p class="mb-1 text-body">${bildirim.icerik}</p>
                                <small class="text-muted">${tarih}</small>
                            </div>
                            <div class="dropdown">
                                <button type="button" class="btn btn-sm btn-icon btn-text-secondary rounded-pill dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                                    <i class="ti ti-dots-vertical ti-md"></i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-end">
                                    ${bildirim.durum === 'Okunmamis' ? `<a class="dropdown-item" href="javascript:void(0)" onclick="bildirimYoneticisi.bildirimOkunduIsaretle(${bildirim.id})">Okundu İşaretle</a>` : ''}
                                    <a class="dropdown-item text-danger" href="javascript:void(0)" onclick="bildirimYoneticisi.bildirimSil(${bildirim.id})">Sil</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </li>
        `;
    }

    bildirimIconGetir(tur) {
        const iconMap = {
            'Sistem': 'ti ti-settings',
            'Bilgi': 'ti ti-info-circle',
            'Basari': 'ti ti-check',
            'Uyari': 'ti ti-alert-triangle',
            'Hata': 'ti ti-x'
        };
        return iconMap[tur] || 'ti ti-bell';
    }

    bildirimRenkGetir(tur) {
        const colorMap = {
            'Sistem': 'bg-label-info',
            'Bilgi': 'bg-label-primary',
            'Basari': 'bg-label-success',
            'Uyari': 'bg-label-warning',
            'Hata': 'bg-label-danger'
        };
        return colorMap[tur] || 'bg-label-secondary';
    }

    async bildirimOkunduIsaretle(bildirimId) {
        try {
            this.log(`Bildirim okundu işaretleniyor: ${bildirimId}`);
            
            const response = await fetch(`/api/bildirimler/${bildirimId}/okundu`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const bildirim = this.bildirimler.find(b => b.id === bildirimId);
            if (bildirim) {
                bildirim.durum = 'Okunmus';
                this.okunmamisSayisiHesapla();
                this.UIGuncelle();
            }

            this.log('Bildirim başarıyla okundu işaretlendi');

        } catch (error) {
            console.error('[BildirimYoneticisi] Bildirim okundu işaretleme hatası:', error);
            alert('Bildirim okundu işaretlenirken hata oluştu');
        }
    }

    async tumunuOkunduIsaretle() {
        try {
            this.log('Tüm bildirimler okundu işaretleniyor...');
            
            const response = await fetch('/api/bildirimler/tumunu-okundu', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            this.bildirimler.forEach(bildirim => {
                bildirim.durum = 'Okunmus';
            });
            
            this.okunmamisSayisiHesapla();
            this.UIGuncelle();

            this.log('Tüm bildirimler başarıyla okundu işaretlendi');

        } catch (error) {
            console.error('[BildirimYoneticisi] Tümünü okundu işaretleme hatası:', error);
            alert('Bildirimler okundu işaretlenirken hata oluştu');
        }
    }

    async bildirimSil(bildirimId) {
        try {
            this.log(`Bildirim siliniyor: ${bildirimId}`);
            
            const response = await fetch(`/api/bildirimler/${bildirimId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            this.bildirimler = this.bildirimler.filter(b => b.id !== bildirimId);
            this.okunmamisSayisiHesapla();
            this.UIGuncelle();

            this.log('Bildirim başarıyla silindi');

        } catch (error) {
            console.error('[BildirimYoneticisi] Bildirim silme hatası:', error);
            alert('Bildirim silinirken hata oluştu');
        }
    }

    tumBildirimleriGoster() {
        this.log('Tüm bildirimler sayfasına yönlendiriliyor...');
        alert('Tüm bildirimler sayfası henüz hazır değil');
    }

    async testBildirimiOlustur() {
        try {
            this.log('Test bildirimi oluşturuluyor...');
            
            const response = await fetch('/api/bildirimler/test', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                this.log('Test bildirimi başarıyla oluşturuldu');
                await this.bildirimleriyukle();
            }

        } catch (error) {
            console.error('[BildirimYoneticisi] Test bildirimi oluşturma hatası:', error);
        }
    }

    fallbackBildirimGoster() {
        this.log('Fallback bildirim gösteriliyor...');
        
        this.bildirimler = [{
            id: -1,
            baslik: 'Bağlantı Hatası',
            icerik: 'Bildirimler yüklenirken hata oluştu. Lütfen sayfayı yenileyin.',
            tur: 'Hata',
            durum: 'Okunmamis',
            createDate: new Date().toISOString()
        }];
        
        this.okunmamisSayisiHesapla();
        this.UIGuncelle();
    }

    destroy() {
        if (this.updateInterval) {
            clearInterval(this.updateInterval);
            this.updateInterval = null;
        }
        this.initialized = false;
        this.log('Bildirim sistemi durduruldu');
    }
}

// Global instance oluştur
const bildirimYoneticisi = new BildirimYoneticisi();

// Sayfa yüklendiğinde başlat
document.addEventListener('DOMContentLoaded', function() {
    bildirimYoneticisi.init();
});

// Global erişim için
window.bildirimYoneticisi = bildirimYoneticisi;
