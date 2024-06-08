import { useEffect, useState } from 'react'
import './partnerprofile.css'
import axios from 'axios';

export default function PartnerProfile() {
    const partnerId = sessionStorage.getItem('DeliveryPartnerUserId');
    const partnerToken=sessionStorage.getItem('DeliveryPartnerToken');
    const [activeTab, setActiveTab] = useState('profile');
    const [orders, setOrders] = useState([]);
    const [orderStatuses, setOrderStatuses] = useState([]);
    const [profile, setProfile]=useState({});

    const handleTabChange = (tab) => {
        setActiveTab(tab);
    };

    //fetch profile details
    useEffect(()=>{
        const fetchPartnerDetails = async () => {
            try {
                const response = await axios.get(`http://localhost:5249/api/DeliveryPartner/GetDetails?partnerId=${partnerId}`);
                setProfile(response.data);
            } catch (error) {
                console.log(error);
            }
        };

        fetchPartnerDetails();
    },[]);

    //fetch orders
    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await axios.get(`http://localhost:5249/api/DeliveryPartner/GetAllOrders?partnerId=${partnerId}`);
                setOrders(response.data);
                setOrderStatuses(response.data.map(order => ({ orderId: order.orderId, status: order.status })));
                console.log(orders);

            } catch (error) {
                console.error('Error fetching orders:', error);
            }
        };

        fetchOrders();
    }, []);

    //handle changes in profile details
    const handleProfileChange = (fieldName, value) => {
        setProfile(prevState => ({
            ...prevState,
            [fieldName]: value
        }));
    };

    //update changes in profile in the system
    const editPartnerDetails = () => {
        var requestOptions = {
            method: 'PUT',
            // headers: { 'Authorization': 'Bearer ' + customerToken },
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(profile)
        };

        fetch("http://localhost:5249/api/DeliveryPartner/UpdateDetails", requestOptions)
            .then(r => r.json())
            .then(alert("Details updated successfully!"))
            .catch(e => console.log(e));
    };

    //change order status
    const handleChangestatus=(orderId)=>{
        try {
            axios.put(`http://localhost:5249/api/DeliveryPartner/ChangeOrderStatus?orderId=${orderId}`);
            const updatedOrders = orders.map(order => {
                if (order.orderId === orderId) {
                    return { ...order, status: 'delivered' };
                }
                return order;
            });
            setOrders(updatedOrders);
            // Update the status for the selected order in orderStatuses
            setOrderStatuses(prevStatuses => prevStatuses.map(status => {
                if (status.orderId === orderId) {
                    return { ...status, status: 'delivered' };
                }
                return status;
            }));
            alert("Status updated successfully");
        } catch (error) {
            console.error('Error changing order status:', error);
            alert("A problem occured while updating the status");
        }
    };

    return (
        <>
            <br /><br /><br /><br />
            <div className='profile-container'>
                <div className="tabs">
                    <button
                        className={activeTab === 'profile' ? 'active' : ''}
                        onClick={() => handleTabChange('profile')}
                    >
                        Profile Details
                    </button>
                    <button
                        className={activeTab === 'orders' ? 'active' : ''}
                        onClick={() => handleTabChange('orders')}
                    >
                        Orders
                    </button>
                    {/* <button
                        className={activeTab === 'changeStatus' ? 'active' : ''}
                        onClick={() => handleTabChange('changeStatus')}
                    >
                        Change Order Status
                    </button> */}
                </div>

                <div className='tab-content'>
                    {activeTab === 'profile' && (
                        <div className='cards-container-1'>
                            <div className='first-card-1'>
                                <div className="imgbox">
                                    {/* You can include user avatar here if available */}
                                    <img
                                        src="/UserProfile.jpg"
                                        alt="User Avatar"
                                    />
                                </div>
                                <div className="content">
                                    <h2>Profile</h2>
                                    <div className='detail-field'>
                                        <label htmlFor='Name'>Name:&nbsp;&nbsp;</label>
                                        <input className='first-text' type='text' value={profile.name} onChange={(e)=>handleProfileChange('name', e.target.value)}/>
                                        {/* value={customerDetails.name} onChange={(e) => handleProfileChange('name', e.target.value)} */}
                                    </div>
                                    <div className='detail-field'>
                                        <label htmlFor='Name'>Email:&nbsp;&nbsp;</label>
                                        <input className='first-text' type='text' value={profile.email} onChange={(e)=>handleProfileChange('email', e.target.value)}/>
                                    </div>
                                    <div className='detail-field'>
                                        <label htmlFor='Name'>Phone:&nbsp;&nbsp;</label>
                                        <input className='first-text' type='text' value={profile.phone} onChange={(e)=>handleProfileChange('phone', e.target.value)}/>
                                    </div>
                                    <div className='detail-field'>
                                        <label htmlFor='Name'>UserName:&nbsp;&nbsp;</label>
                                        <input className='first-text' type='text' value={profile.userName} disabled />
                                    </div>
                                </div>
                                <button className="edit-button" onClick={editPartnerDetails}>Edit</button> {/* Edit button */}
                            </div>
                        </div>
                    )}
                    {activeTab === 'orders' && (
                        <div className='change-status-tab'>
                            <h2>Change Order Status</h2>
                            <table>
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Date</th>
                                        <th>Amount</th>
                                        <th>Status</th>
                                        <th>Customer</th>
                                        <th>Change Status</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {orders.slice(0).reverse().map((order, index) => (
                                        <tr key={order.orderId}>
                                            <td>{order.orderId}</td>
                                            <td>{new Date(order.orderDate).toLocaleDateString()}</td>
                                            <td>Rs. {order.amount.toFixed(2)}</td>
                                            <td>{order.status}</td>
                                            <td>{order.customer.name}</td>
                                            <td>
                                                {order.status === 'On the way' || order.status==='on the way' ? (
                                                    <>
                                                        <select value={orderStatuses[index].status} onChange={(e) => handleChangestatus(order.orderId)} >
                                                            <option value={order.status}>{order.status}</option>
                                                            <option value="delivered">Delivered</option>
                                                        </select>
                                                        <button>
                                                            <i className="bx bx-edit"></i> Change Status
                                                        </button>
                                                    </>
                                                ) : (<>
                                                    <select value={orderStatuses[index].status} onChange={(e) => handleChangestatus(order.orderId)} disabled>
                                                        <option value={order.status}>{order.status}</option>
                                                        <option value="delivered">Delivered</option>
                                                    </select>
                                                    <button>
                                                        <i className="bx bx-edit"></i> Change Status
                                                    </button>
                                                </>)}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </div>
        </>
    )
};