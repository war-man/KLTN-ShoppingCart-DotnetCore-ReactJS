import React, { Component } from 'react';
import CartItem from './cartItem/CartItem';
import {connect} from 'react-redux';
import {removeItem, addQuantity, subtractQuantity} from '../../action/cartsAction'

 class ListCart extends Component {
    handleRomoveItem(id){
        this.props.removeItem(id);
    }
    handleAddQuantity(id){
        this.props.addQuantity(id);
    }
    handleSubQuantity(id){
        this.props.subtractQuantity(id);
    }
    render() {
        //render list cart
        
        const listItem = (
            this.props.carts.length ? this.props.carts.map(item=>{
                return <CartItem key={item.id} item={item}
                removeItem={this.handleRomoveItem.bind(this)}
                addQuantity={this.handleAddQuantity.bind(this)}
                subQuantity={this.handleSubQuantity.bind(this)}
                >
                </CartItem>
            })
            : <h2>Bạn chưa thêm giỏ hàng</h2>
        )
            
        
        
        
        return (
            <div style={{width:'73%'}}>
                {
                    listItem
                }
            </div>
        )
    }
}
const mapStateToProps = (state) =>{
    return {
        carts: state.carts.carts
    }
}
const mapDispatchToProps = (dispatch) => {
    return{
        removeItem: (id) => {dispatch(removeItem(id))},
        addQuantity: (id) => {dispatch(addQuantity(id))},
        subtractQuantity: (id) => {dispatch(subtractQuantity(id))}
    }
}
export default connect(mapStateToProps, mapDispatchToProps)(ListCart);