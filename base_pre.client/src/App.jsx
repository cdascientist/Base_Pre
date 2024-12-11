import { useState, useEffect } from 'react';
import CompanyProfile from './CompanyProfile';

export default function App() {
    const [companies, setCompanies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [scrollIndex, setScrollIndex] = useState(0);
    const [isVisible, setIsVisible] = useState(false);
    const [selectedProfile, setSelectedProfile] = useState(null);

    useEffect(() => {
        fetch('http://localhost:5000/api/ModelDbInits/GetCustomers', {
            method: 'POST'
        })
            .then(response => response.json())
            .then(data => {
                setCompanies(data);
                setLoading(false);
                setTimeout(() => setIsVisible(true), 100);
            })
            .catch(err => {
                console.error('Error:', err);
                setLoading(false);
            });
    }, []);

    const handleCompanySelect = (customerId) => {
        setSelectedProfile(customerId);
    };

    const staggeredFade = (delay) => ({
        opacity: isVisible ? 1 : 0,
        transition: `opacity 0.8s ease-in-out ${delay}s`,
    });

    if (selectedProfile !== null) {
        return <CompanyProfile customerId={selectedProfile} />;
    }

    if (loading) {
        return (
            <div style={{
                position: 'absolute',
                top: '50%',
                left: '50%',
                transform: 'translate(-50%, -50%)',
                color: '#57b3c0',
                fontSize: '28px',
                letterSpacing: '6px',
                textShadow: '0 0 15px rgba(87, 179, 192, 0.7)',
                zIndex: 9999
            }}>
                LOADING...
            </div>
        );
    }

    return (
        <div style={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            zIndex: 9995
        }}>
            {/* Title */}
            <div style={{
                position: 'absolute',
                top: '5%',
                left: '50%',
                transform: 'translateX(-50%)',
                color: '#57b3c0',
                fontSize: '24px',
                letterSpacing: '6px',
                whiteSpace: 'nowrap',
                fontWeight: '500',
                textShadow: '0 0 15px rgba(87, 179, 192, 0.7)',
                zIndex: 9997,
                ...staggeredFade(0)
            }}>
                COMPANY DIRECTORY
            </div>

            {/* Company Name Box */}
            <div style={{
                position: 'absolute',
                top: '25%',
                left: '50%',
                transform: 'translateX(-50%)',
                width: '100%',
                maxWidth: '800px',
                zIndex: 9996,
                ...staggeredFade(0.3)
            }}>
                <div
                    onClick={() => handleCompanySelect(companies[scrollIndex]?.customerId)}
                    style={{
                        padding: '25px',
                        border: '2.5px solid #57b3c0',
                        backgroundColor: 'rgba(87, 179, 192, 0.1)',
                        textAlign: 'center',
                        letterSpacing: '4px',
                        fontSize: '28px',
                        fontWeight: '600',
                        textTransform: 'uppercase',
                        zIndex: 9998,
                        cursor: 'pointer',
                        transition: 'all 0.3s ease'
                    }}
                    onMouseEnter={(e) => {
                        e.currentTarget.style.backgroundColor = 'rgba(87, 179, 192, 0.2)';
                        e.currentTarget.style.boxShadow = '0 0 15px rgba(87, 179, 192, 0.3)';
                    }}
                    onMouseLeave={(e) => {
                        e.currentTarget.style.backgroundColor = 'rgba(87, 179, 192, 0.1)';
                        e.currentTarget.style.boxShadow = 'none';
                    }}
                >
                    <span style={{
                        color: '#FFFFFF',
                        textShadow: `
                            0 0 20px rgba(87, 179, 192, 0.9),
                            0 0 30px rgba(87, 179, 192, 0.7),
                            0 0 40px rgba(87, 179, 192, 0.5)
                        `,
                        fontWeight: '500'
                    }}>
                        {companies[scrollIndex]?.companyName || 'UNNAMED'}
                    </span>
                </div>
            </div>

            {/* Navigation Buttons */}
            <div style={{
                position: 'absolute',
                top: '110%',
                left: '50%',
                transform: 'translateX(-50%)',
                display: 'flex',
                gap: '30px',
                zIndex: 9999,
                width: '100%',
                maxWidth: '800px',
                justifyContent: 'center',
                ...staggeredFade(0.6)
            }}>
                <button
                    onClick={() => setScrollIndex(prev => Math.max(0, prev - 1))}
                    disabled={scrollIndex === 0}
                    style={{
                        padding: '10px 25px',
                        border: '2px solid #57b3c0',
                        backgroundColor: 'transparent',
                        color: '#57b3c0',
                        cursor: 'pointer',
                        fontSize: '18px',
                        letterSpacing: '2px',
                        opacity: scrollIndex === 0 ? '0.5' : '1',
                        transition: 'all 0.3s ease'
                    }}
                    onMouseEnter={(e) => {
                        if (scrollIndex !== 0) {
                            e.currentTarget.style.borderColor = '#57b3c0';
                            e.currentTarget.style.color = '#FFFFFF';
                            e.currentTarget.style.boxShadow = '0 0 15px rgba(87, 179, 192, 0.5)';
                            e.currentTarget.style.backgroundColor = 'rgba(87, 179, 192, 0.2)';
                        }
                    }}
                    onMouseLeave={(e) => {
                        e.currentTarget.style.borderColor = '#57b3c0';
                        e.currentTarget.style.color = '#57b3c0';
                        e.currentTarget.style.boxShadow = 'none';
                        e.currentTarget.style.backgroundColor = 'transparent';
                    }}
                >
                    PREVIOUS
                </button>
                <button
                    onClick={() => setScrollIndex(prev => Math.min(companies.length - 1, prev + 1))}
                    disabled={scrollIndex === companies.length - 1}
                    style={{
                        padding: '10px 25px',
                        border: '2px solid #57b3c0',
                        backgroundColor: 'transparent',
                        color: '#57b3c0',
                        cursor: 'pointer',
                        fontSize: '18px',
                        letterSpacing: '2px',
                        opacity: scrollIndex === companies.length - 1 ? '0.5' : '1',
                        transition: 'all 0.3s ease'
                    }}
                    onMouseEnter={(e) => {
                        if (scrollIndex !== companies.length - 1) {
                            e.currentTarget.style.borderColor = '#57b3c0';
                            e.currentTarget.style.color = '#FFFFFF';
                            e.currentTarget.style.boxShadow = '0 0 15px rgba(87, 179, 192, 0.5)';
                            e.currentTarget.style.backgroundColor = 'rgba(87, 179, 192, 0.2)';
                        }
                    }}
                    onMouseLeave={(e) => {
                        e.currentTarget.style.borderColor = '#57b3c0';
                        e.currentTarget.style.color = '#57b3c0';
                        e.currentTarget.style.boxShadow = 'none';
                        e.currentTarget.style.backgroundColor = 'transparent';
                    }}
                >
                    NEXT
                </button>
            </div>
        </div>
    );
}