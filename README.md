<h1 align="center">🧩 Net6SampleProject</h1>

<p align="center">
  Bu proje, N-tier mimarisiyle yapılandırılmış, katmanlar arası oosely coupled yapı sunan, 
  <strong>Entity Framework Core</strong> ile veri erişimi sağlayan, 
  <strong>IMemoryCache</strong> ile performans optimizasyonu yapılmış, 
  test edilebilirlik ve sürdürülebilirlik ön planda tutularak geliştirilen modüler ve ölçeklenebilir bir <strong>.NET 6</strong> örnek projesidir.
</p>

---

## 📁 Proje Yapısı

Bu çözüm, katmanlı mimari prensiplerine uygun olarak aşağıdaki projelerden oluşur:

- **Net6SampleProject.API**  
  → RESTful servislerin sunulduğu Web API katmanı.

- **Net6SampleProject.Web**  
  → ASP.NET Core MVC kullanılarak oluşturulmuş kullanıcı arayüzü.

- **Net6SampleProject.Service**  
  → İş kurallarını içeren katman. Repository ile API arasında köprü kurar.

- **Net6SampleProject.Repository**  
  → Entity Framework Core kullanılarak veritabanı işlemlerinin yapıldığı katman.

- **Net6SampleProject.Caching**  
  → `IMemoryCache` interface'i ile çalışan Memory Cache implementasyonlarını içerir. Performans artırımı ve sorgu optimizasyonu sağlar.

- **Net6SampleProject.Core**  
  → Domain modelleri, interface'ler, enum'lar ve ortak bileşenleri barındıran temel katman.
